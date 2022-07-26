﻿using MovieMatch.Movies;
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
        }
        public override async Task<WatchLaterDto> CreateAsync(CreateUpdateWatchLaterDto input)
        {
            var exist = await _movieRepository.AnyAsync(input.MovieId);
            if (!exist)
            {
                var existingMovieFromApi = _movieAppService.GetAsync(input.MovieId);
                var createMovieDto = new CreateMovieDto(
                    existingMovieFromApi.Result.Id,
                    existingMovieFromApi.Result.Title,
                    existingMovieFromApi.Result.PosterPath,
                    existingMovieFromApi.Result.Overview);
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
        public override async Task<PagedResultDto<WatchLaterDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            var query = from watchLater in queryable
                        select new { watchLater };

            query = query
               .Skip(input.SkipCount)
               .Take(input.MaxResultCount);

            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of WatchLaterDto objects
            var watchLaterDtos = queryResult.Select(x =>
            {
                var watchLaterDto = ObjectMapper.Map<WatchLater, WatchLaterDto>(x.watchLater);
                watchLaterDto.UserId = x.watchLater.UserId;
                watchLaterDto.MovieId = x.watchLater.MovieId;
                watchLaterDto.Id=x.watchLater.Id;

                return watchLaterDto;
            }).Where(x=>x.UserId==_currentUser.Id).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<WatchLaterDto>(
             totalCount,
             watchLaterDtos
         );

        }


    }
}
