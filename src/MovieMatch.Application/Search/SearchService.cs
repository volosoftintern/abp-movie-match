using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.Search
{
    public class SearchService : MovieMatchAppService, ISearchService
    {
        
        private readonly IApiMovieRequest _movieApi;
        public SearchService()
        {   
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
        }


        public async Task<SearchResponseDto<MovieDto>> GetMovies(SearchMovieDto input)
        {
            var response= await _movieApi.SearchByTitleAsync(input.Name,input.CurrentPage);

            return new SearchResponseDto<MovieDto>(response.PageNumber, response.TotalResults, response.TotalPages, ObjectMapper.Map<IReadOnlyList<MovieInfo>,IReadOnlyList<MovieDto>>(response.Results));

        }

        public async Task<SearchResponseDto<MovieDto>> GetPopularMovies(PopularMovieDto input)
        {
            var response=await _movieApi.GetPopularAsync(input.CurrentPage);

            return new SearchResponseDto<MovieDto>(response.PageNumber, response.TotalResults, response.TotalPages, ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results));
        }
    }
}
