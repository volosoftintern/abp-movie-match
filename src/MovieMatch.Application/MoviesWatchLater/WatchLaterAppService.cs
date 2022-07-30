using MovieMatch.Movies;
using MovieMatch.MoviesWatchLater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Authorization;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi;
using Volo.Abp.ObjectMapping;
using Movie = DM.MovieApi.MovieDb.Movies.Movie;

namespace MovieMatch.MoviesWatchLater
{
    public class WatchLaterAppService :
        CrudAppService<
            WatchLater,
            WatchLaterDto, //Used to show WatchLaters
            Guid, //Primary key of the WatchLater entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateWatchLaterDto>, //Used to create/update a WatchLater
        IWatchLaterAppService //implement the IWatchLaterAppService

    {
        private readonly IMovieAppService _movieAppService;
        private readonly IMovieRepository _movieRepository;
        private readonly IWatchLaterRepository _watchLaterRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IApiMovieRequest _movieApi;
        public WatchLaterAppService(IRepository<WatchLater, Guid> repository,
    IMovieAppService movieAppService,
    IWatchLaterRepository watchLaterRepository,
    IMovieRepository movieRepository,
    ICurrentUser currentUser) : base(repository)
        {
            _watchLaterRepository = watchLaterRepository;
            _movieAppService = movieAppService;
            _movieRepository = movieRepository;
            _currentUser = currentUser;
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
        }
        [Authorize]
        public override async Task<WatchLaterDto> CreateAsync(CreateUpdateWatchLaterDto input)
        {
            var exist = await _movieRepository.AnyAsync(input.MovieId);
            if (!exist)
            {
                var existingMovieFromApi = await _movieAppService.GetAsync(input.MovieId);
                var createMovieDto = new CreateMovieDto(
                    existingMovieFromApi.Id,
                    existingMovieFromApi.Title,
                    existingMovieFromApi.PosterPath,
                    existingMovieFromApi.Overview,
                    true
                    );
                await _movieAppService.CreateAsync(createMovieDto);
            }

            var isExistMovieInMyList = await _watchLaterRepository.FindByIdAsync(input.MovieId);
            if (isExistMovieInMyList == null)
            {
                var createMovie = new WatchLater(
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
            var movies=await _watchLaterRepository.GetListAsync(x => x.UserId == id);
            return movies.Count;
        }
    }
}
