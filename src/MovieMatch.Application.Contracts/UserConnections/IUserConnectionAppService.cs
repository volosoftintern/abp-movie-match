
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Identity;

namespace MovieMatch.UserConnections
{
          public interface IUserConnectionAppService :

        IApplicationService
    {
        Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input);
        Task<PagedResultDto<FollowerDto>> GetFollowingAsync(GetUsersFollowInfo input);
        Task<PagedResultDto<FollowerDto>> GetFollowersAsync(GetUsersFollowInfo input);
        Task<int> GetFollowersCount(string username);
        Task<int> GetFollowingCount(string username);
        Task<List<Guid>> GetFirstAsync();
        Task<string> GetPhotoAsync(string userName);
        Task SetPhotoAsync(string userName,string name);
        Task FollowAsync(Guid id, bool isActive);
        Task UploadAsync(IFormFile file);
        Task UnFollowAsync(Guid id, bool isActive);
        Task<UserInformationDto> GetUserInfoAsync(string username);
      

    }
}
