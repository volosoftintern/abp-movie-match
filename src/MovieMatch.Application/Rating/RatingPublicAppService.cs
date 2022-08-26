using Microsoft.AspNetCore.Authorization;
using MovieMatch.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Public;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;
using CmsUserDto = Volo.CmsKit.Public.Comments.CmsUserDto;
using CommentDto = MovieMatch.Comments.CommentDto;

namespace MovieMatch.Rating;

[Authorize]
[RequiresGlobalFeature(typeof(RatingsFeature))]
public class RatingPublicAppService : CmsKitPublicAppServiceBase, IRatingPublicAppService
{
    protected ICommentPublicAppService CommentPublicAppService;
    protected IRatingRepository RatingRepository { get; }
    public ICmsUserLookupService CmsUserLookupService { get; }
    protected RatingManager RatingManager { get; }
    protected ICommentRepository CommentRepository { get; }
    protected IIdentityUserRepository _userRepository { get; }
    public int pageNumber;

    private const int maxItem = 5;

    public RatingPublicAppService(
        ICommentRepository commentRepository,
        IRatingRepository ratingRepository,
        ICmsUserLookupService cmsUserLookupService,
        RatingManager ratingManager,
        ICommentPublicAppService commentPublicAppService, IIdentityUserRepository identityUserRepository)
    {
        _userRepository = identityUserRepository;
        CommentRepository = commentRepository;
        RatingRepository = ratingRepository;
        CmsUserLookupService = cmsUserLookupService;
        RatingManager = ratingManager;
        CommentPublicAppService = commentPublicAppService;
        pageNumber = new int();
    }


    public virtual async Task<RatingDto> CreateAsync(string entityType, string entityId,
        CreateUpdateRatingInput input)
    {
        var userId = CurrentUser.GetId();
        var user = await CmsUserLookupService.GetByIdAsync(userId);

        var rating = await RatingManager.SetStarAsync(user, entityType, entityId, input.StarCount);

        return ObjectMapper.Map<Volo.CmsKit.Ratings.Rating, RatingDto>(rating);
    }


    public virtual async Task DeleteAsync(string entityType, string entityId)
    {
        var rating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, CurrentUser.GetId());

        if (rating.CreatorId != CurrentUser.GetId())
        {
            throw new AbpAuthorizationException();
        }

        await RatingRepository.DeleteAsync(rating.Id);
    }

    public virtual async Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType,
        string entityId)
    {
        var ratings = await RatingRepository.GetGroupedStarCountsAsync(entityType, entityId);

        var userRatingOrNull = CurrentUser.IsAuthenticated
            ? await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, CurrentUser.GetId())
            : null;

        var ratingWithStarCountDto = new List<RatingWithStarCountDto>();

        foreach (var rating in ratings)
        {
            ratingWithStarCountDto.Add(
                new RatingWithStarCountDto
                {
                    StarCount = rating.StarCount,
                    Count = rating.Count,
                    IsSelectedByCurrentUser = userRatingOrNull != null && userRatingOrNull.StarCount == rating.StarCount
                });
        }

        return ratingWithStarCountDto;
    }
    private MyCmsUserDto GetAuthorAsDtoFromCommentList(List<CommentWithAuthorQueryResultItem> comments, Guid commentId)
    {
        return ObjectMapper.Map<CmsUser, MyCmsUserDto>(comments.Single(c => c.Comment.Id == commentId).Author);
    }

    public virtual async Task<List<CommentWithStarsDto>> GetCommentsWithRatingAsync(string entityType, string entityId, int currPage)
    {
        var comments = await CommentRepository.GetListWithAuthorsAsync(entityType, entityId);

        var parentComments = comments
            .Where(c => c.Comment.RepliedCommentId == null)
            .OrderByDescending(c => c.Comment.CreationTime)
            .Select(c =>
            {
                var comment = ObjectMapper.Map<Comment, CommentWithStarsDto>(c.Comment);
                comment.Author = ObjectMapper.Map<CmsUser, MyCmsUserDto>(c.Author);
                return comment;
            })
            .Skip((currPage-1)*maxItem).Take(maxItem)
            .ToList();

        var semaphore = new SemaphoreSlim(1);
        parentComments.ForEach( (x) =>
        {
            x.Author = GetAuthorAsDtoFromCommentList(comments, x.Id);
            x.Path = string.IsNullOrEmpty(x.Author.Path) ? ProfilePictureConsts.DefaultPhotoPath : x.Author.Path;
            x.Replies = comments
                .Where(c => c.Comment.RepliedCommentId == x.Id)
                .Select(c => ObjectMapper.Map<Comment, Comments.CommentDto>(c.Comment)).ToList();

            x.Replies.ForEach(async (r) =>
            {
                try
                {
                    await semaphore.WaitAsync();
                    var user = await _userRepository.GetAsync(r.CreatorId);
                    r.Author = GetAuthorAsDtoFromCommentList(comments, r.Id);
                    r.Author.Path = user.GetProperty(ProfilePictureConsts.PhotoProperty, ProfilePictureConsts.DefaultPhotoPath);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            
        });


        var res= (await Task.WhenAll(parentComments.Select(async (c) =>
        {
            try
            {
                await semaphore.WaitAsync();
                var userRating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, c.CreatorId);

                return new CommentWithStarsDto
                {
                    Author = new MyCmsUserDto { 
                        Id = c.Author.Id, 
                        Name = c.Author.Name, 
                        UserName = c.Author.UserName, 
                        Path = c.Path 
                    },
                    ConcurrencyStamp = c.ConcurrencyStamp,
                    CreationTime = c.CreationTime,
                    EntityId = entityId,
                    EntityType = entityType,
                    Id = c.Id,
                    Replies = c.Replies == null ? new List<CommentDto>() : c.Replies,
                    Text = c.Text,
                    CreatorId = c.Author.Id,
                    StarsCount = userRating == null ? 0 : userRating.StarCount
                };

            }
            finally
            {
                semaphore.Release();
            }

        }))).ToList();

        semaphore.Dispose();

        return res;
    }
}


