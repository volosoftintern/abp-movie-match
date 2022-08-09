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
using Volo.Abp.Identity;

namespace MovieMatch.UserConnections
{
    public class UserRepository : EfCoreRepository<MovieMatchDbContext, IdentityUser>, IUserRepository
    {
        public UserRepository(IDbContextProvider<MovieMatchDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(IEnumerable<Guid> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindByName(string Name)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Set<IdentityUser>()
                .Where(u => EF.Property<string>(u, "Photo") == Name)
                .FirstOrDefaultAsync();
        }

        public Task<IdentityUser> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
