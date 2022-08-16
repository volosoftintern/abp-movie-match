using DM.MovieApi;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMatch.Search
{
    public class SearchService : MovieMatchAppService, ISearchService
    {
        private readonly IWatchLaterRepository _watchLaterRepository;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly IApiMovieRequest _movieApi;
        private readonly IMovieAppService _movieAppService;
        
        public SearchService(IWatchLaterRepository watchLaterRepository,
            IWatchedBeforeRepository watchedBeforeRepository,IMovieAppService movieAppService)
        {
            _watchedBeforeRepository = watchedBeforeRepository;
            _watchLaterRepository = watchLaterRepository;
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
            _movieAppService = movieAppService;
        }


        public async Task<SearchResponseDto<MovieDto>> GetMovies(SearchMovieDto input)//Implement Polly
        {
            IReadOnlyList<MovieDto> result;
            ApiSearchResponse<MovieInfo> response;

            if (string.IsNullOrEmpty(input.Name))
            {
                response = await _movieApi.GetPopularAsync(input.CurrentPage);
            }
            else
            {
                response = await _movieApi.SearchByTitleAsync(input.Name, input.CurrentPage);//
            }
            
            result = ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);

            await BackupMoviesAsync(result);

            foreach (var item in result)
            {
                item.IsActiveWatchLater = (await _watchLaterRepository.GetQueryableAsync()).Any(x => x.UserId == CurrentUser.Id && x.MovieId==item.Id);
                item.IsActiveWatchedBefore = (await _watchedBeforeRepository.GetQueryableAsync()).Any(x => x.UserId == CurrentUser.Id && x.MovieId == item.Id);
            } 

            return new SearchResponseDto<MovieDto>(response.PageNumber, response.TotalResults, response.TotalPages,result);

        }

        private async Task BackupMoviesAsync(IReadOnlyList<MovieDto> movies)
        {
            foreach (var item in movies)
            {
                if(!(await _movieAppService.AnyAsync(item.Id)))
                {
                    await _movieAppService.CreateAsync(
                        new CreateMovieDto(item.Id,item.Title,item.PosterPath,item.Overview)
                        );
                }
            }
        }


    }
}
