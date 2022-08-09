using Microsoft.AspNetCore.Mvc;
using MovieMatch.Posts;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MovieMatch.Web.Components.Post
{
    [Widget(
        ScriptFiles = new[] { "/Components/Post/Default.js" },
        StyleFiles = new[] { "/Components/Post/Default.css" }
       )]
    public class PostViewComponent : AbpViewComponent
    {

        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/Post/Default.cshtml");
        }

       
    }
}
