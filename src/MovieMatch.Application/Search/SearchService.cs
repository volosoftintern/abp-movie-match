using DM.MovieApi.MovieDb.Movies;
using IMDbApiLib;
using IMDbApiLib.Models;
using Microsoft.Extensions.Options;
using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.ObjectMapping;

namespace MovieMatch.Search
{
    public class SearchService : MovieMatchAppService, ISearchService
    {
        private readonly MovieApiService _movieApiService;

        public SearchService(MovieApiService movieApiService)
        {
            _movieApiService=movieApiService;
        }


        public async Task<IReadOnlyList<MovieDto>> GetMovies(SearchMovieDto input)
        {
            
            
            var result = await _movieApiService.SearchMovieAsync(input.Name);
            

            
            //if (!string.IsNullOrEmpty(result.ErrorMessage)) throw new UserFriendlyException(result.ErrorMessage);

            return ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(result);

        }
    }
}
