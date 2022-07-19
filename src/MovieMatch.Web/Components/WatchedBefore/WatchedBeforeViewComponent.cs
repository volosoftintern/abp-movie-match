using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MovieMatch.Web.Components.WatchedBefore
{
    public class WatchedBeforeViewComponent:AbpViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/WatchedBefore/Default.cshtml");
        }
    }
}
