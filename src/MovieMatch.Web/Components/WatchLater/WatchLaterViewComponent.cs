using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MovieMatch.Web.Components.WatchLater
{
    [Widget(
        ScriptFiles = new[] { "/Components/WatchLater/Default.js" },
        StyleFiles = new[] { "/Components/WatchLater/def.css" }
        )]
    public class WatchLaterViewComponent : AbpViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/WatchLater/Default.cshtml");
        }
    }

}