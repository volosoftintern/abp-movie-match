using Microsoft.EntityFrameworkCore;
using MovieMatch.EntityFrameworkCore;
using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace MovieMatch.MoviesWatchedBefore
{
    public class EfCoreWatchedBeforeRepository : EfCoreRepository<MovieMatchDbContext, WatchedBefore, Guid>, IWatchedBeforeRepository
    {
        private readonly ICurrentUser _currentUser;

        public EfCoreWatchedBeforeRepository(IDbContextProvider<MovieMatchDbContext> dbContextProvider, ICurrentUser currentUser) : base(dbContextProvider)
        {
            _currentUser = currentUser;
        }

        public async Task<WatchedBefore> FindByIdAsync(int id)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(watchedBefore => watchedBefore.UserId == _currentUser.Id).FirstOrDefaultAsync(x=>x.MovieId==id);

        }
    }
}
