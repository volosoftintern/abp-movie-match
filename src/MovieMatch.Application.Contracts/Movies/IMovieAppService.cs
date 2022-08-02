using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.Movies
{
    public interface IMovieAppService:IApplicationService
    {
        Task<MovieDetailDto> GetAsync(int id);
        Task<MovieDto> CreateAsync(CreateMovieDto movie);
        Task<PagedResultDto<MovieDto>> GetWatchedBeforeListAsync(PagedAndSortedResultRequestDto input);
        Task<PagedResultDto<MovieDto>> GetWatchLaterListAsync(PagedAndSortedResultRequestDto input);
        Task<DirectorDto> GetDirector(int directorId);
        Task<bool> AnyAsync(int id);
        Task<MovieDto> GetFromDbAsync(int id);
    }
}
