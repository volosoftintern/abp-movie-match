using Microsoft.EntityFrameworkCore;
using MovieMatch.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace MovieMatch.UserConnections
{
    public class EFCoreUserConnectionRepository :
        EfCoreRepository<MovieMatchDbContext, UserConnection>, IUserConnectionRepository


    {
        public EFCoreUserConnectionRepository(IDbContextProvider<MovieMatchDbContext> dbContextProvider) : base(dbContextProvider)
        {


        }
        public async Task<IQueryable<UserConnection>> GetListAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return dbSet;
        }

        public async Task<IQueryable<UserConnection>> GetFollowersAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return dbSet.Where(c => c.FollowingId == userId);
        }

        public async Task<IQueryable<UserConnection>> GetFollowingAsync(Guid userId)
        {
            var dbSet=await GetDbSetAsync();
            return dbSet.Where(c => c.FollowerId == userId);
        }
    }
}
