using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace MovieMatch.UserConnections
{
    public interface IUserConnectionRepository : IRepository<UserConnection>
    {
        Task<IQueryable<UserConnection>> GetFollowersAsync(Guid userId);
        Task<IQueryable<UserConnection>> GetFollowingAsync(Guid userId);
    }
}
