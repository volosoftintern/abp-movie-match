using MovieMatch.Movies;
using MovieMatch.UserConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace MovieMatch.Posts
{
    public class PostService :
        MovieMatchAppService, IPostService
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IIdentityUserAppService _userAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IMovieAppService _movieAppService;
        private readonly ICommentRepository _commentRepository;
        private readonly IRatingRepository _ratingRepository;

        public PostService(
            IUserConnectionRepository userConnectionRepository,
            IIdentityUserAppService userAppService,
            IMovieAppService movieAppService,
            ICurrentUser currentUser,
            ICommentRepository commentRespository,
            IRatingRepository ratingRepository
            )
        {
            _userConnectionRepository = userConnectionRepository;
            _userAppService = userAppService;
            _movieAppService = movieAppService;
            _currentUser = currentUser;
            _commentRepository = commentRespository;
            _ratingRepository = ratingRepository;
        }

        public async Task<PagedResultDto<PostDto>> GetFeedAsync(PostFeedDto input)
        {

            var followingUserIds = (await _userConnectionRepository.GetQueryableAsync()).
                Where(x => x.FollowerId.Equals(input.UserId)).Select(x => x.FollowingId);

            var comments = await _commentRepository.
            GetListAsync(entityType: nameof(Movie));

            comments = comments.
                Where(x => x.Comment.CreatorId.Equals(input.UserId) ||
                followingUserIds.Any(f => f.Equals(x.Author.Id))).ToList();

            int totalCount = comments.Count();

            var ratings = (await _ratingRepository.GetListAsync())
                .Where(x => comments.Any(
                    c => c.Comment.CreatorId.Equals(x.CreatorId) &&
                    c.Comment.EntityId.Equals(x.EntityId))
                );

            var result = comments
               .OrderByDescending(x => x.Comment.CreationTime)
               .Skip(input.SkipCount)
               .Take(input.MaxCount).ToList();

            var semaphore = new SemaphoreSlim(1);

            var posts =await Task.WhenAll(result.Select(async (x) =>
            {
                try
                {
                    await semaphore.WaitAsync();

                    var user = ObjectMapper.Map<CmsUser, IdentityUserDto>(x.Author);

                    var rate = ratings.GetRating(x.Comment.CreatorId, x.Comment.EntityId);

                    var movie = await _movieAppService.GetFromDbAsync(int.Parse(x.Comment.EntityId));

                    return new PostDto()
                    {   
                        Comment = x.Comment.Text,
                        CreationTime = x.Comment.CreationTime,
                        MovieId = int.Parse(x.Comment.EntityId),
                        UserId = x.Comment.CreatorId,
                        Movie = movie,
                        User = user,
                        Rate = rate
                    };
                }
                finally
                {
                    semaphore.Release();
                }

            }));

            semaphore.Dispose();
            return new PagedResultDto<PostDto>(totalCount, posts);
        }
    }

    public static class RatingExtension
    {
        public static int GetRating(this IEnumerable<Volo.CmsKit.Ratings.Rating> ratings, Guid creatorId, string entityId)
        {
            var rating = ratings.Where(x => x.EntityId.Equals(entityId) &&
                x.CreatorId.Equals(creatorId))
            .FirstOrDefault();

            if (rating == null) return 0;
            return rating.StarCount;
        }
    }
}
