﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.Movies
{
    public interface IMovieAppService:IApplicationService
    {
        Task<MovieDetailDto> GetAsync(int id);
        Task<MovieDto> CreateAsync(CreateMovieDto movie);
        Task<PagedResultDto<MovieDto>> GetWatchedBeforeListAsync(MoviePagedAndSortedResultRequestDto input);
        Task<PagedResultDto<MovieDto>> GetWatchLaterListAsync(PagedAndSortedResultRequestDto input);
        Task<PersonDto> GetPersonAsync(int directorId,bool isDirector=false);
        Task<bool> AnyAsync(int id);
        Task<MovieDto> GetFromDbAsync(int id);
        Task<MovieDto> GetMovieAsync(int id);
        Task<IReadOnlyList<MovieDto>> GetSimilarMoviesAsync(int id);
        Task<PagedResultDto<MovieDto>> GetPersonMoviesAsync(PersonMovieRequestDto input);
    }
}
