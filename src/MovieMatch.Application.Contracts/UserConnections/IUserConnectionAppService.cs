using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace MovieMatch.UserConnections
{
          public interface IUserConnectionAppService :

        IApplicationService
    {
        Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input);
        Task<PagedResultDto<FollowerDto>> GetFollowingAsync(GetIdentityUsersInput input);
        Task<PagedResultDto<FollowerDto>> GetFollowersAsync(GetIdentityUsersInput input);
        Task<List<Guid>> GetFirstAsync();
        Task FollowAsync(Guid id, bool isActive);
        Task UnFollowAsync(Guid id, bool isActive);

    }
}
