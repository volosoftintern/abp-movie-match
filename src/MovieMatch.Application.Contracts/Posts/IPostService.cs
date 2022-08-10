using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.Posts
{
    public interface IPostService: IApplicationService
    {
        Task<PagedResultDto<PostDto>> GetFeedAsync(PostFeedDto input);
    }
}
