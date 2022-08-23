using Microsoft.AspNetCore.Authorization;
using MovieMatch.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public RatingPublicAppService(
        ICommentRepository commentRepository,
        IRatingRepository ratingRepository,
        ICmsUserLookupService cmsUserLookupService,
        RatingManager ratingManager,
        ICommentPublicAppService commentPublicAppService,IIdentityUserRepository identityUserRepository)
    {
        _userRepository = identityUserRepository;
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
    private MyCmsUserDto GetAuthorAsDtoFromCommentList(List<CommentWithAuthorQueryResultItem> comments, Guid commentId)
    {
        
        return ObjectMapper.Map<CmsUser, MyCmsUserDto>(comments.Single(c => c.Comment.Id == commentId).Author);
        ////  return ObjectMapper.Map<CmsUser, MyCmsUserDto>(comments.Single(c => c.Author.Id == commentId).Author);
    }
    [Authorize]
    public virtual async Task<List<CommentWithStarsDto>> GetCommentsWithRatingAsync(string entityType, string entityId,int currPage)
    {
        var total = (currPage*5);
        

        var comments = await CommentRepository.GetListWithAuthorsAsync(entityType, entityId);

        var parentComments = comments
.Where(c => c.Comment.RepliedCommentId == null).OrderByDescending(c => c.Comment.CreationTime)
.Select(c => ObjectMapper.Map<Comment, CommentWithStarsDto>(c.Comment)).Skip(0).Take(total)
.ToList();

        
        
      
        foreach (var item in parentComments)
        {
            if (item != null)
            {

                var user = await _userRepository.GetAsync(item.CreatorId);
                item.Path = user.GetProperty("Photo").ToString();
              //  item.Replies.Path= user.GetProperty("Photo").ToString();
            }
        }

        foreach (var parentComment in parentComments)
        {
           
            
                parentComment.Author = GetAuthorAsDtoFromCommentList(comments, parentComment.Id);
               //  = new  { Id = authordto.Id,Name=authordto.Name,Path=parentComment.Path,UserName=authordto.UserName,Surname=authordto.Surname };

            

            parentComment.Replies = comments
                .Where(c => c.Comment.RepliedCommentId == parentComment.Id)
                .Select(c => ObjectMapper.Map<Comment, Comments.CommentDto>(c.Comment))
                .ToList();


            foreach (var reply in parentComment.Replies)
            {
                var user =await _userRepository.GetAsync(reply.CreatorId);

                reply.Author = GetAuthorAsDtoFromCommentList(comments, reply.Id);
                reply.Author.Path = user.GetProperty("Photo").ToString();
            }
        }

        var commentWithStarCountDto = new List<CommentWithStarsDto>();

        foreach (var comment in parentComments)
        {
          
            if (comment.Author != null)
            {
               var userRating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, comment.CreatorId);
                if (userRating == null)
                {
                    commentWithStarCountDto.Add(
                    new CommentWithStarsDto
                    {
                        Author = new MyCmsUserDto { Id = comment.Author.Id, Name = comment.Author.Name, UserName = comment.Author.UserName, Path = comment.Path.ToString() },
                        ConcurrencyStamp = comment.ConcurrencyStamp,
                        CreationTime = comment.CreationTime,
                        EntityId = entityId,
                        EntityType = entityType,
                        Id = comment.Id,
                        Replies = comment.Replies,
                        Text = comment.Text,
                        CreatorId = comment.Author.Id,
                        StarsCount = 0,
                    });
                }
                else
                {
                    commentWithStarCountDto.Add(
                    new CommentWithStarsDto
                    {
                        Author = { Id = comment.Author.Id, Name = comment.Author.Name, UserName = comment.Author.UserName, Path = comment.Author.Path.ToString() },
                        ConcurrencyStamp = comment.ConcurrencyStamp,
                        CreationTime = comment.CreationTime,
                        EntityId = entityId,
                        EntityType = entityType,
                        Id = comment.Id,
                        Replies = comment.Replies,
                        Text = comment.Text,
                        CreatorId = comment.Author.Id,
                        StarsCount = userRating.StarCount,


                    });
                }

            }
            
        }
        
        return commentWithStarCountDto;
    }


}


