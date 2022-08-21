using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace MovieMatch.UserConnections
{
    public class UserConnectionManager:DomainService
    {
        
        private readonly ICurrentUser _currentUser;

       

        public UserConnectionManager( ICurrentUser currentUser)
        {
            _currentUser = currentUser;

        }
        public async Task<UserConnection> CreateAsync(Guid followingId)
        {
            return new UserConnection(followingId, (Guid)_currentUser.Id)
            {

                FollowingId = followingId,
                FollowerId = (Guid)_currentUser.Id,
                
            };

        }
    }
}
