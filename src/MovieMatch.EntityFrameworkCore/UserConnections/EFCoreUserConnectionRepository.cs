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
        private readonly ICurrentUser _currentUser;
    
        public EFCoreUserConnectionRepository(ICurrentUser currentUser, IDbContextProvider<MovieMatchDbContext> dbContextProvider) : base(dbContextProvider)
        {
                
            _currentUser = currentUser;
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
        public async Task<IQueryable<IdentityUser>> GetUsersListAsync(
           int skipCount,
           int maxResultCount,string filter=null
       )
        {
            var query = await GetDbSetAsync();


            var dbContext = await GetDbContextAsync();
            var userqueryable = dbContext.Set<IdentityUser>().AsQueryable();
    
            var users =  userqueryable.Where(x=>x.Id!=_currentUser.Id).PageBy(skipCount,maxResultCount).WhereIf(!string.IsNullOrEmpty(filter),x=>x.Name.Contains(filter)).OrderBy(x=>x.Name);
            return users;
            


        }
    }
}
