using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace MovieMatch.Movies
{
    public class MovieManager:DomainService
    {
        public Movie Create(int id,string title,string posterPath,string overView)
         {
            return new Movie(id,title,posterPath,overView);
         } 
    }
}
