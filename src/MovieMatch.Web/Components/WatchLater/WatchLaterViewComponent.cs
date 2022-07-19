using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

public class WatchLaterViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/WatchLater/Default.cshtml");
    }
}
