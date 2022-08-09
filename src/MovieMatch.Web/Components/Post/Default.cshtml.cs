using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.Posts;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace MovieMatch.Web.Components.Post
{
    public class DefaultModel : PageModel
    {
   
        public ListResultDto<PostDto> ListResultDto { get; set; }

        private readonly IPostService _postService;
        private readonly ICurrentUser _curentUser;

        public DefaultModel(IPostService postService,ICurrentUser currentUser)
        {
            _postService = postService;
            _curentUser = currentUser;
        }

        public async Task OnGetAsync()
        {
            ListResultDto = await _postService.GetFeedAsync(new PostFeedDto { UserId=(Guid)_curentUser.Id});

        }


    }
}
