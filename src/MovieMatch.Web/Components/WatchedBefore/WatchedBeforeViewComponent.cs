using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MovieMatch.Web.Components.WatchedBefore
{
    [Widget(ScriptFiles = new[] { "/Components/WatchedBefore/Default.js" })]
    public class WatchedBeforeViewComponent:AbpViewComponent
    {
        public WatchedBeforeViewComponent()
        {

        }
        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/WatchedBefore/Default.cshtml");
        }

       
    }
}
