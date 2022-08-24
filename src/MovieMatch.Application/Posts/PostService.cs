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
using AutoMapper.Internal.Mappers;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;

namespace MovieMatch.Posts
{
    [Authorize]
    public class PostService :
        MovieMatchAppService, IPostService
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Post, int> _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly IWatchLaterRepository _watchLaterRepository;

        public PostService(IRepository<Post, int> repository,
            IUserConnectionRepository userConnectionRepository,
            IMovieRepository movieRepository,
            ICurrentUser currentUser,
            ICommentRepository commentRespository,
            IRatingRepository ratingRepository,
            IIdentityUserRepository identityUserRepository,
            IWatchedBeforeRepository watchedBeforeRepository,
            IWatchLaterRepository watchLaterRepository

            )
        {
            _watchLaterRepository = watchLaterRepository;
            _watchedBeforeRepository = watchedBeforeRepository;
            _userConnectionRepository = userConnectionRepository;
            _movieRepository = movieRepository;
            _currentUser = currentUser;
            _repository = repository;
            _commentRepository = commentRespository;
            _ratingRepository= ratingRepository;
            _identityUserRepository = identityUserRepository;
        }
        public async Task<MovieDto> GetFromDbAsync(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            var resp = ObjectMapper.Map<Movie, MovieDto>(movie);

            resp.IsActiveWatchedBefore = (await _watchedBeforeRepository.GetQueryableAsync()).Any(x => x.MovieId == resp.Id && x.UserId == CurrentUser.Id);
            resp.IsActiveWatchLater = (await _watchLaterRepository.GetQueryableAsync()).Any(x => x.MovieId == resp.Id && x.UserId == CurrentUser.Id);

            return resp;
        }
        public async Task<IdentityUserDto> GetUser(Guid id)
        {
            var user= await _identityUserRepository.GetAsync(id);
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

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
            var re=result.SelectMany(x => x.Comment.EntityId);
            
            var posts = result.Select(async (x) => new PostDto()
            {
                Comment = x.Comment.Text,
                CreationTime=x.Comment.CreationTime,
                MovieId = int.Parse(x.Comment.EntityId),
                UserId = x.Comment.CreatorId,
                Movie=await GetFromDbAsync(int.Parse(x.Comment.EntityId)),
                User= await GetUser(x.Comment.CreatorId),
                Rate=ratingSet.FirstOrDefault(r=>r.CreatorId==x.Comment.CreatorId && int.Parse(r.EntityId)==int.Parse(x.Comment.EntityId))==null?0: ratingSet.FirstOrDefault(r => r.CreatorId == x.Comment.CreatorId && int.Parse(r.EntityId) == int.Parse(x.Comment.EntityId)).StarCount
            }).Select(t=>t.Result).ToList();

            return new PagedResultDto<PostDto>(totalCount, posts);
        }

    }
}
