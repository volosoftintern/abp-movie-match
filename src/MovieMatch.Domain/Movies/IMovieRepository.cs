using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch.Movies
{
    public interface IMovieRepository : IRepository<Movie, int>
    {
        Task<Movie> FindByIdAsync(int id);
        Task<bool> AnyAsync(int id);
    }
}
