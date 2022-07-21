using Volo.Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi;

namespace MovieMatch.Movies
{
    public class MovieAppService : MovieMatchAppService, IMovieAppService
    {
        private readonly IApiMovieRequest _movieApi;

        public MovieAppService(MovieApiService movieApiService)
        {
            
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
        }

        public async Task<MovieDetailDto> GetAsync(int id)
        {
            var response = await _movieApi.FindByIdAsync(id);
            return ObjectMapper.Map<Movie, MovieDetailDto>(response.Item);
        }
    }
}
