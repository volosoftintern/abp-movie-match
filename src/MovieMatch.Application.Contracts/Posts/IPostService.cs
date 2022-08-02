using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.Posts
{
    public interface IPostService: IApplicationService
    {
        Task<ListResultDto<PostDto>> GetFeedAsync(PostFeedDto input);
        Task<PostDto> CreateAsync(CreatePostDto input);
    }
}
