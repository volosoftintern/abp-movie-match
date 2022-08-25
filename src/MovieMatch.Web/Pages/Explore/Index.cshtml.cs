using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.UserConnections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;


namespace MovieMatch.Web.Pages.Explore
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUserConnectionAppService _userConnectionService;
        public PagedResultDto<FollowerDto> Users;
        public List<IdentityUserDto> RecommendedUsers;
        public IndexModel(IUserConnectionAppService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public int UserCount { get; set; }

        public async Task  OnGet()
        {
            //  filepath= await _userConnectionService.GetPhotoAsync(_currentUser.UserName);
            Users = await _userConnectionService.GetListAsync(new GetIdentityUsersInput());
            RecommendedUsers = await _userConnectionService.GetRecommendedUsersList(new GetIdentityUsersInput());
            UserCount = (int)Users.TotalCount;
        }
    }
}
