using Microsoft.AspNetCore.Authorization;
using MovieMatch.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.GlobalFeatures;
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

namespace MovieMatch.Rating;

[RequiresGlobalFeature(typeof(RatingsFeature))]
public class RatingPublicAppService : CmsKitPublicAppServiceBase, IRatingPublicAppService
{
    protected ICommentPublicAppService CommentPublicAppService;
    protected IRatingRepository RatingRepository { get; }
    public ICmsUserLookupService CmsUserLookupService { get; }
    protected RatingManager RatingManager { get; }
    protected ICommentRepository CommentRepository { get; }
    public int pageNumber;

    public RatingPublicAppService(
        ICommentRepository commentRepository,
        IRatingRepository ratingRepository,
        ICmsUserLookupService cmsUserLookupService,
        RatingManager ratingManager,
        ICommentPublicAppService commentPublicAppService)
    {
        CommentRepository = commentRepository;
        RatingRepository = ratingRepository;
        CmsUserLookupService = cmsUserLookupService;
        RatingManager = ratingManager;
        CommentPublicAppService = commentPublicAppService;
        pageNumber = new int();
    }

    [Authorize]
    public virtual async Task<RatingDto> CreateAsync(string entityType, string entityId,
        CreateUpdateRatingInput input)
    {
        var userId = CurrentUser.GetId();
        var user = await CmsUserLookupService.GetByIdAsync(userId);

        var rating = await RatingManager.SetStarAsync(user, entityType, entityId, input.StarCount);

        return ObjectMapper.Map<Volo.CmsKit.Ratings.Rating, RatingDto>(rating);
    }

    [Authorize]
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
    private CmsUserDto GetAuthorAsDtoFromCommentList(List<CommentWithAuthorQueryResultItem> comments, Guid commentId)
    {
        return ObjectMapper.Map<CmsUser, CmsUserDto>(comments.Single(c => c.Comment.Id == commentId).Author);
    }
    [Authorize]
    public virtual async Task<List<CommentWithStarsDto>> GetCommentsWithRatingAsync(string entityType, string entityId)
    {
       // var currentPageIndex = 1;
        

        var comments = await CommentRepository.GetListWithAuthorsAsync(entityType, entityId);
        var parentComments = comments
        .Where(c => c.Comment.RepliedCommentId == null).OrderBy(c => c.Comment.CreationTime)
        .Select(c => ObjectMapper.Map<Comment, CommentWithStarsDto>(c.Comment))
        .ToList();
        foreach (var parentComment in parentComments)
        {
            parentComment.Author = GetAuthorAsDtoFromCommentList(comments, parentComment.Id);

            parentComment.Replies = comments
                .Where(c => c.Comment.RepliedCommentId == parentComment.Id)
                .Select(c => ObjectMapper.Map<Comment, Comments.CommentDto>(c.Comment))
                .ToList();

            foreach (var reply in parentComment.Replies)
            {
                reply.Author = GetAuthorAsDtoFromCommentList(comments, reply.Id);
            }
        }

        var commentWithStarCountDto = new List<CommentWithStarsDto>();

        foreach (var comment in parentComments)
        {
            var userRating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, comment.CreatorId);
            if (userRating == null)
            {
                commentWithStarCountDto.Add(
                new CommentWithStarsDto
                {
                    Author = comment.Author,
                    ConcurrencyStamp = comment.ConcurrencyStamp,
                    CreationTime = comment.CreationTime,
                    EntityId = entityId,
                    EntityType = entityType,
                    Id = comment.Id,
                    Replies = comment.Replies,
                    Text = comment.Text,
                    CreatorId = comment.CreatorId,
                    StarsCount = 0,
                });
            }
            else
            {
                commentWithStarCountDto.Add(
                new CommentWithStarsDto
                {
                    Author = comment.Author,
                    ConcurrencyStamp = comment.ConcurrencyStamp,
                    CreationTime = comment.CreationTime,
                    EntityId = entityId,
                    EntityType = entityType,
                    Id = comment.Id,
                    Replies = comment.Replies,
                    Text = comment.Text,
                    CreatorId = comment.CreatorId,
                    StarsCount = userRating.StarCount,

                });
            }
        }
        
        return commentWithStarCountDto;
    }


}



//var listOfComments = CommentPublicAppService.GetListAsync(entityType, entityId).Result.Items;
//var commentWithStarCountDto = new List<CommentWithStarsDto>();
//        foreach (var comment in listOfComments)
//        {
//            var userRating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, comment.CreatorId);
//            if (userRating == null)
//            {
//                commentWithStarCountDto.Add(
//                new CommentWithStarsDto
//                {
//                    Author = comment.Author,
//                    ConcurrencyStamp = comment.ConcurrencyStamp,
//                    CreationTime = comment.CreationTime,
//                    EntityId = entityId,
//                    EntityType = entityType,
//                    Id = comment.Id,
//                    Replies = comment.Replies,
//                    Text = comment.Text,
//                    CreatorId = comment.CreatorId,
//                    StarsCount = 0
//                });
//            }
//            else
//{
//    commentWithStarCountDto.Add(
//    new CommentWithStarsDto
//    {
//        Author = comment.Author,
//        ConcurrencyStamp = comment.ConcurrencyStamp,
//        CreationTime = comment.CreationTime,
//        EntityId = entityId,
//        EntityType = entityType,
//        Id = comment.Id,
//        Replies = comment.Replies,
//        Text = comment.Text,
//        CreatorId = comment.CreatorId,
//        StarsCount = userRating.StarCount
//    });
//}
            
//        }
//        return commentWithStarCountDto;
