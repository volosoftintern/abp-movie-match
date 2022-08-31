        using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DM.MovieApi.MovieDb.Movies;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MovieMatch.Comments;
using MovieMatch.Movies;
using MovieMatch.Rating;
using MovieMatch.Web.Components.Rating;
using MovieMatch.Web.Controllers;
using Nancy.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Commenting;
using Volo.CmsKit.Public.Web.Renderers;
using Movie = MovieMatch.Movies.Movie;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace MovieMatch.Web.Components.Commenting;

[ViewComponent(Name = "CmsCommenting")]
[Widget(
    ScriptTypes = new[] { typeof(Commenting.CommentingScriptBundleContributor) },
    StyleTypes = new[] { typeof(CommentingStyleBundleContributor) },
   StyleFiles =new[] { "/Components/Commenting/Default.css" },
    RefreshUrl = "/CmsKitPublicWidgets/Commenting",
    AutoInitialize = true
)]
public class CommentingViewComponent : AbpViewComponent
{
    public ICommentPublicAppService CommentPublicAppService { get; }
    public IMarkdownToHtmlRenderer MarkdownToHtmlRenderer { get; }
    public AbpMvcUiOptions AbpMvcUiOptions { get; }

    private readonly IMovieAppService _movieAppService;
    private readonly MovieMatch.Rating.IRatingPublicAppService _ratingPublicAppService;

    public CommentingViewComponent(
        ICommentPublicAppService commentPublicAppService,
        IOptions<AbpMvcUiOptions> options,
        IMarkdownToHtmlRenderer markdownToHtmlRenderer,
        IMovieAppService movieAppService,
        MovieMatch.Rating.IRatingPublicAppService ratingPublicAppService)
    {
        _movieAppService = movieAppService;
        CommentPublicAppService = commentPublicAppService;
        MarkdownToHtmlRenderer = markdownToHtmlRenderer;
        AbpMvcUiOptions = options.Value;
        _ratingPublicAppService = ratingPublicAppService;
    }
    public virtual async Task<IViewComponentResult> InvokeAsync(
        string entityType,
        string entityId,
        int currPage)
    {

        var comments = (await CommentPublicAppService
            .GetListAsync(entityType, entityId)).Items;
        
        var loginUrl = $"{AbpMvcUiOptions.LoginUrl}?returnUrl={HttpContext.Request.Path.ToString()}&returnUrlHash=#cms-comment_{entityType}_{entityId}";
       
        var id = int.Parse(entityId);
        
        var viewModel = new CommentingViewModel
        {
            EntityId = entityId,
            EntityType = entityType,
            LoginUrl = loginUrl,
            Comments = comments.OrderByDescending(i => i.CreationTime).ToList(),
            Movie =await _movieAppService.GetAsync(id), 
            CommentsWithStars =await  _ratingPublicAppService.GetCommentsWithRatingAsync(entityType, entityId,currPage)
        };

        await ConvertMarkdownTextsToHtml(viewModel);


        return View("/Components/Commenting/Default.cshtml", viewModel);
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

    public class CommentingViewModel
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string LoginUrl { get; set; }
        public List<CommentWithDetailsDto> Comments { get; set; }
        public List<CommentWithStarsDto> CommentsWithStars { get; set; }
        public Dictionary<Guid, string> RawCommentTexts { get; set; }
        public MovieDetailDto Movie { get; set; }
    }
}


