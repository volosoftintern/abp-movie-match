using Microsoft.AspNetCore.Mvc;
using MovieMatch.UserConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MovieMatch.Controllers
{
    public class UserConnectionController:AbpController
    {
        private readonly IUserConnectionAppService _userConnectionAppService;

        public UserConnectionController(IUserConnectionAppService userConnectionAppService)
        {
            _userConnectionAppService = userConnectionAppService;
        }
        [HttpGet]
        public async Task<UserInformationDto> GetUserProfiles(string username)
        {
            UserInformationDto userInformation=new UserInformationDto();
           userInformation=await _userConnectionAppService.GetUserInfoAsync(username);
            return (userInformation);
            

        }

    }
}
