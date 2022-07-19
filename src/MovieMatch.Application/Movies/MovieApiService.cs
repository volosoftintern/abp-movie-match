using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using IMDbApiLib;
using IMDbApiLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MovieMatch.Movies
{
    public class MovieApiService:ITransientDependency
    {
        private readonly IApiMovieRequest _movieApi;
        
        public MovieApiService()
        {
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
        }

        public async Task<IReadOnlyList<MovieInfo>> SearchMovieAsync(string name)
        {
            var response= await _movieApi.SearchByTitleAsync(name);
            return response.Results;
        }

    }
}
