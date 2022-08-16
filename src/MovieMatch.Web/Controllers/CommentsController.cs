using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc;

namespace MovieMatch.Web.Controllers
{
    public class CommentsController:AbpController
    {
        public IActionResult MyViewComponent()
        {
            return ViewComponent("CmsCommenting");
        }
    }
}
