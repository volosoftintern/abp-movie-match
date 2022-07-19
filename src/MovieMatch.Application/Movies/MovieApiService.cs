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
        private readonly ApiLib _apiLib;

        public MovieApiService()
        {
            _apiLib=new ApiLib(MovieApiConstants.ApiKey);
        }

        public async Task<SearchData> SearchMovieAsync(string name)
        {
             var result= await _apiLib.SearchMovieAsync(name);
            return result;  
        }

    }
}
