using Microsoft.AspNetCore.Authorization;
using MovieMatch.Movies;
using MovieMatch.UserConnections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

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

        public PostService(IRepository<Post, int> repository,
            IUserConnectionRepository userConnectionRepository,
            IIdentityUserAppService userAppService,
            IMovieAppService movieAppService,
            ICurrentUser currentUser)
        {
            _userConnectionRepository = userConnectionRepository;
            _userAppService = userAppService;
            _movieAppService = movieAppService;
            _currentUser = currentUser;
            _repository = repository;
        }

        public async Task<PostDto> CreateAsync(CreatePostDto input)
        {

            if (!(await _movieAppService.AnyAsync(input.MovieId)))
            {

                var movie = await _movieAppService.GetAsync(input.MovieId);

                await _movieAppService.CreateAsync(new CreateMovieDto(movie.Id, movie.Title, movie.PosterPath, movie.Overview));

            }
            
            var post = new Post(input.UserId, input.MovieId, input.Comment, input.Rate);
            post = await _repository.InsertAsync(post, true);
            var postDto=ObjectMapper.Map<Post, PostDto>(post);
            postDto.MovieTitle = input.MovieTitle;
            postDto.Username = _currentUser.UserName;

            return postDto;

        }

        public async Task<ListResultDto<PostDto>> GetFeedAsync(PostFeedDto input)
        {

            var queryable = await _repository.GetQueryableAsync();
            var following = (await _userConnectionRepository.GetQueryableAsync()).Where(x => x.FollowerId == input.UserId).Select(x => x.FollowingId);

            var query= queryable.Where(x => x.UserId == input.UserId || following.Contains(x.UserId));

            query = query
               .OrderByDescending(x => x.Id);
            
            var queryResult = await AsyncExecuter.ToListAsync(query);

            var posts = ObjectMapper.Map<List<Post>, List<PostDto>>(queryResult);
            foreach (var item in posts)
            {
                item.MovieTitle = (await _movieAppService.GetFromDbAsync(item.MovieId)).Title;
                item.Username = (await _userAppService.GetAsync(item.UserId)).UserName;
            }

            return new ListResultDto<PostDto>(posts);

        }

    }
}
