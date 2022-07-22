using Volo.Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi;
using DM.MovieApi.MovieDb.Genres;

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
            var credits= await _movieApi.GetCreditsAsync(id);
            var director = credits.Item.CrewMembers.FirstOrDefault((c) => c.Job == "Director");
            var stars = credits.Item.CastMembers.Take(3);//stars
            
            var movieDetail=ObjectMapper.Map<Movie, MovieDetailDto>(response.Item);
            
            movieDetail.Director = ObjectMapper.Map<MovieCrewMember,MovieMemeberDto>(director);
            movieDetail.Stars= ObjectMapper.Map<IEnumerable<MovieCastMember>, IEnumerable<MovieMemeberDto>>(stars);
            //movieDetail.Genres= ObjectMapper.Map<IReadOnlyList<Genre>, IReadOnlyList<MovieGenreDto>>(response.Item.Genres);


            return movieDetail;
        }
    }
}
