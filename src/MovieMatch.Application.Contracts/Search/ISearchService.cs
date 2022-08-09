using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.Search
{
    public interface ISearchService:IApplicationService
    {
        Task<SearchResponseDto<MovieDto>> GetMovies(SearchMovieDto input);
    }
}
