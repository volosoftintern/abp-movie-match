using Microsoft.EntityFrameworkCore;
using MovieMatch.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MovieMatch.Movies
{
    public class EfCoreMovieRepository : EfCoreRepository<MovieMatchDbContext, Movie, int>, IMovieRepository
    {

        public EfCoreMovieRepository(IDbContextProvider<MovieMatchDbContext> dbContextProvider): base(dbContextProvider)
        {
        }
        public async Task<bool> AnyAsync(int id)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.AnyAsync(movie => movie.Id == id);
        }

        public async Task<Movie> FindByIdAsync(int id)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(movie => movie.Id == id);
        }

    }
}
