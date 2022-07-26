using Microsoft.EntityFrameworkCore;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchLater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace MovieMatch.EntityFrameworkCore.MoviesWatchLater
{
    public class EfCoreWatchLaterRepository : EfCoreRepository<MovieMatchDbContext, WatchLater, Guid>, IWatchLaterRepository
    {
        private readonly ICurrentUser _currentUser;

        public EfCoreWatchLaterRepository(IDbContextProvider<MovieMatchDbContext> dbContextProvider,
            ICurrentUser currentUser) : base(dbContextProvider)
        {
            _currentUser = currentUser;
        }

        public async Task<WatchLater> FindByIdAsync(int id)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(watchLater => watchLater.UserId == _currentUser.Id).FirstOrDefaultAsync(x => x.MovieId == id);

        }
    }

}
