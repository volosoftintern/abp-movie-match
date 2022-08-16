using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.UserConnections;
using System.Threading.Tasks;
using Volo.Abp.Identity;


namespace MovieMatch.Web.Pages.Explore
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUserConnectionAppService _userConnectionService;

        public IndexModel(IUserConnectionAppService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public int UserCount { get; set; }

        public async Task  OnGet()
        {
            //  filepath= await _userConnectionService.GetPhotoAsync(_currentUser.UserName);
            var result = await _userConnectionService.GetListAsync(new GetIdentityUsersInput());
            UserCount = (int)result.TotalCount;
        }
    }
}
