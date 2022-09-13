using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Public.Web.Renderers;
using static MovieMatch.Web.Components.Commenting.CommentingViewComponent;

namespace MovieMatch.Web.Controllers
{
    //[Route("CmsKitPublicWidgets")]
    public class CommentsController:AbpController
    {
        [HttpGet]
        public IActionResult MyViewComponent(string entityType,string entityId,int currPage)
        {
            return ViewComponent("CmsCommenting", new { entityType, entityId,currPage});

        }
    }

}
