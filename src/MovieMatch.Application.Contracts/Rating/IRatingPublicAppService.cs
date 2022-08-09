using MovieMatch.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Public.Ratings;

namespace MovieMatch.Rating;

public interface IRatingPublicAppService : IApplicationService
{
    Task<RatingDto> CreateAsync(string entityType, string entityId, CreateUpdateRatingInput input);

    Task DeleteAsync(string entityType, string entityId);

    Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType, string entityId);
    Task<List<CommentWithStarsDto>> GetCommentsWithRatingAsync(string entityType, string entityId);
}