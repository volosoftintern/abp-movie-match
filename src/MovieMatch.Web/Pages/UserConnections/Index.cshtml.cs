using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace MovieMatch.Web.Pages.UserConnections
{
    public class IndexModel : PageModel
    {
    //    private readonly CurrentUser _currentUser;
    //    public IdentityUser user;
    //    public IndexModel(CurrentUser currentUser)
    //    {
    //      _currentUser = currentUser;
    //    }

        public void OnGet()
        {
            //user.Name= _currentUser.Name;
            //return RedirectToPage(user.Name);
        }
    }
}
