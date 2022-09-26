using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MovieMatch.Web.Components.Navbar
{
    [Widget(
        ScriptFiles = new[] { "/Components/Navbar/Default.js" },
   StyleFiles = new[] { "/Components/Navbar/Default.css" }
)]
    public class NavbarViewComponent:AbpViewComponent
    {
        public NavbarViewComponent()
        {

        }
        public IViewComponentResult Invoke()
        {
            var viewModel = new ViewModel
            {
                EntityId = "2",
                EntityType = "movie",
                LoginUrl = "ss.com",
            };
            return View("/Components/Navbar/Default.cshtml",viewModel);
        }
        public class ViewModel
        {
            public string EntityType { get; set; }
            public string EntityId { get; set; }
            public string LoginUrl { get; set; }
        }
    }
}
