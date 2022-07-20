using Volo.Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.MovieApi.MovieDb.Movies;

namespace MovieMatch.Movies
{
    public class MovieAppService : MovieMatchAppService, IMovieAppService
    {
        private readonly MovieApiService _movieApiService;
        public MovieAppService(MovieApiService movieApiService)
        {
            _movieApiService = movieApiService;
        }

        public async Task<MovieDetailDto> GetAsync(int id)
        {
            var result= await _movieApiService.GetMovieAsync(id);
            return ObjectMapper.Map<Movie, MovieDetailDto>(result);
        }
    }
}
