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
        public IMarkdownToHtmlRenderer MarkdownToHtmlRenderer { get; }
        public CommentsController(IMarkdownToHtmlRenderer markdownToHtmlRenderer)
        {
            MarkdownToHtmlRenderer = markdownToHtmlRenderer;
        }
        [HttpGet]
        //[Route("Commenting")]
        public IActionResult MyViewComponent(string entityType,string entityId,int currPage)
        {
            return ViewComponent("CmsCommenting", new { entityType, entityId,currPage});
            //await ConvertMarkdownTextsToHtml(viewModel);
            //return View("/Components/Commenting/Default.cshtml", viewModel);
        }

        private async Task ConvertMarkdownTextsToHtml(CommentingViewModel viewModel)
        {
            viewModel.RawCommentTexts = new Dictionary<Guid, string>();

            foreach (var comment in viewModel.Comments)
            {
                viewModel.RawCommentTexts.Add(comment.Id, comment.Text);
                comment.Text = await MarkdownToHtmlRenderer.RenderAsync(comment.Text, true);

                foreach (var reply in comment.Replies)
                {
                    viewModel.RawCommentTexts.Add(reply.Id, reply.Text);
                    reply.Text = await MarkdownToHtmlRenderer.RenderAsync(reply.Text, true);
                }
            }
        }
    }

}
