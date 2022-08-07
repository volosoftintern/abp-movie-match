using Microsoft.AspNetCore.Authorization;
using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace MovieMatch.MoviesWatchedBefore
{
    public class WatchedBeforeAppService :
        CrudAppService<
            WatchedBefore,
            WatchedBeforeDto, //Used to show WatchedBefores
            Guid, //Primary key of the WatchedBefore entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateWatchedBeforeDto>, //Used to create/update a WatchedBefore
        IWatchedBeforeAppService //implement the IWatchedBeforeAppService
    {
        private readonly IMovieAppService _movieAppService;
        private readonly IMovieRepository _movieRepository;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _usersService;
        private readonly List<IdentityUser> userDtos;
        public WatchedBeforeAppService(IRepository<WatchedBefore, Guid> repository, 
            IMovieAppService movieAppService, 
            IWatchedBeforeRepository watchedBeforeRepository, 
            IMovieRepository movieRepository,
            IIdentityUserRepository usersService,
            ICurrentUser currentUser) :base(repository)
        {
            _watchedBeforeRepository=watchedBeforeRepository;
            _movieAppService = movieAppService;
            _movieRepository = movieRepository;
            _currentUser = currentUser;
            _usersService = usersService;
            userDtos = new List<IdentityUser>();
        }
        [Authorize]
        public override async Task<WatchedBeforeDto> CreateAsync(CreateUpdateWatchedBeforeDto input)
        {
            var exist = await _movieRepository.AnyAsync(input.MovieId);
            if (!exist)
            {
                var existingMovieFromApi= _movieAppService.GetAsync(input.MovieId);
                var createMovieDto=new CreateMovieDto(
                    existingMovieFromApi.Result.Id,
                    existingMovieFromApi.Result.Title,
                    existingMovieFromApi.Result.PosterPath,
                    existingMovieFromApi.Result.Overview,
                    true);
                await _movieAppService.CreateAsync(createMovieDto);
            }
            var isExistMovieInMyList = await _watchedBeforeRepository.FindByIdAsync(input.MovieId);
            if (isExistMovieInMyList==null)
            {
                var createMovie = new WatchedBefore(
                    (Guid)_currentUser.Id,
                    input.MovieId);
                await Repository.InsertAsync(createMovie);
            }
            else
            {
                var movie = await _movieRepository.FindByIdAsync(input.MovieId);
                throw new MovieAlreadyExistsException(movie.Title);
            }
            return null;
        }

        public async Task<int> GetCountAsync(Guid id)
        {
            var movies=await _watchedBeforeRepository.GetListAsync(x=>x.UserId==id);
            return movies.Count;
        }
        public async Task<List<IdentityUserDto>> ListOfUsers(int movieId)
        {
            var usersWatchedBefore = await _watchedBeforeRepository.GetListAsync(x => x.MovieId == movieId);
            if(usersWatchedBefore==null) return null;
            foreach (var item in usersWatchedBefore)
            {
                var users=await _usersService.FindAsync(item.UserId);
                userDtos.Add(users);
            }
            var userList = ObjectMapper.Map<List<IdentityUser>,List<IdentityUserDto>>(userDtos);
            return userList;
        }
    }
}
