using MovieMatch.Movies;
using MovieMatch.UserConnections;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Authorization;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Ratings;

namespace MovieMatch.Posts
{
    [Authorize]
    public class PostService :
        MovieMatchAppService, IPostService
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IIdentityUserAppService _userAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IMovieAppService _movieAppService;
        private readonly IRepository<Post, int> _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly IRatingRepository _ratingRepository;

        public PostService(IRepository<Post, int> repository,
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
            _repository = repository;
            _commentRepository = commentRespository;
            _ratingRepository= ratingRepository;
        }

        public async Task<PagedResultDto<PostDto>> GetFeedAsync(PostFeedDto  input)
        {

            var maxItemCount = await _commentRepository.GetCountAsync();

            var commentSet = await _commentRepository.GetListAsync(entityType: nameof(Movie));
            

            var following = (await _userConnectionRepository.GetQueryableAsync()).Where(x => x.FollowerId == input.UserId).Select(x => x.FollowingId).ToList();
            
            var query = commentSet.Where(x => x.Comment.CreatorId == input.UserId || following.Any(f=>f==x.Comment.CreatorId));            
            int totalCount = query.Count();

            var result= query
               .OrderByDescending(x => x.Comment.CreationTime).Skip(input.SkipCount).Take(input.MaxCount).ToList();

            var ratingSet = (await _ratingRepository.GetListAsync()).Where(x=> commentSet.Any(c=>c.Comment.CreatorId==x.CreatorId && c.Comment.EntityId==x.EntityId));

            var posts = result.Select(async (x) => new PostDto()
            {
                Comment = x.Comment.Text,
                CreationTime=x.Comment.CreationTime,
                MovieId = int.Parse(x.Comment.EntityId),
                UserId = x.Comment.CreatorId,
                Movie= (await _movieAppService.GetFromDbAsync(int.Parse(x.Comment.EntityId))),
                User= (await _userAppService.GetAsync(x.Comment.CreatorId)),
                Rate=ratingSet.FirstOrDefault(r=>r.CreatorId==x.Comment.CreatorId && int.Parse(r.EntityId)==int.Parse(x.Comment.EntityId))==null?0: ratingSet.FirstOrDefault(r => r.CreatorId == x.Comment.CreatorId && int.Parse(r.EntityId) == int.Parse(x.Comment.EntityId)).StarCount
            }).Select(t=>t.Result).ToList();

            return new PagedResultDto<PostDto>(totalCount, posts);
        }

    }
}
