using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace MovieMatch.UserConnections
{
    public interface IUserRepository : IRepository<IdentityUser, Guid>
    {
        
        
            Task<IdentityUser> FindByName(string Name);
        
    }
}
