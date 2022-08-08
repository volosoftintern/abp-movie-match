using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MovieMatch.Web.Components.WatchedBefore;
using Volo.Abp.AspNetCore.Mvc;

namespace MovieMatch.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("Widgets")]
    public class MovieController:AbpController
    {
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("Counters")]
        public IActionResult Counters()
        {
            
            return ViewComponent(typeof(WatchedBeforeViewComponent));
        }
    }
}
