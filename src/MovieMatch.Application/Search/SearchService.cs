using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.Search
{
    public class SearchService : MovieMatchAppService, ISearchService
    {
        private readonly IWatchLaterRepository _watchLaterRepository;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly IApiMovieRequest _movieApi;
        public SearchService(IWatchLaterRepository watchLaterRepository,
            IWatchedBeforeRepository watchedBeforeRepository)
        {
            _watchedBeforeRepository = watchedBeforeRepository;
            _watchLaterRepository = watchLaterRepository;
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
        }


        public async Task<SearchResponseDto<MovieDto>> GetMovies(SearchMovieDto input)
        {
            var response= await _movieApi.SearchByTitleAsync(input.Name,input.CurrentPage);
            var userMoviesWatchLater = await _watchLaterRepository.GetListAsync(x => x.UserId == CurrentUser.Id);
            var userMoviesWatchedBefore = await _watchedBeforeRepository.GetListAsync(x => x.UserId == CurrentUser.Id);
            
            var result = ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);

            foreach (var item in result)
            {
                var check = userMoviesWatchLater.FirstOrDefault(x => x.MovieId == item.Id);
                if (check!=null) {
                    item.IsActiveWatchLater = true;
                }
                else{
                    item.IsActiveWatchLater = false;
                }
            } 
            foreach (var item in result)
            {
                var check = userMoviesWatchedBefore.FirstOrDefault(x => x.MovieId == item.Id);
                if (check!=null) {
                    item.IsActiveWatchedBefore = true;
                }
                else{
                    item.IsActiveWatchedBefore = false;
                }
            }


            return new SearchResponseDto<MovieDto>(response.PageNumber, response.TotalResults, response.TotalPages,result);

        }

        public async Task<SearchResponseDto<MovieDto>> GetPopularMovies(PopularMovieDto input)
        {
            var response=await _movieApi.GetPopularAsync(input.CurrentPage);

            var userMoviesWatchLater = await _watchLaterRepository.GetListAsync(x => x.UserId == CurrentUser.Id);
            var userMoviesWatchedBefore = await _watchedBeforeRepository.GetListAsync(x => x.UserId == CurrentUser.Id);

            var result = ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);

            foreach (var item in result)
            {
                var check = userMoviesWatchLater.FirstOrDefault(x => x.MovieId == item.Id);
                if (check != null)
                {
                    item.IsActiveWatchLater = true;
                }
                else
                {
                    item.IsActiveWatchLater = false;
                }
            }
            foreach (var item in result)
            {
                var check = userMoviesWatchedBefore.FirstOrDefault(x => x.MovieId == item.Id);
                if (check != null)
                {
                    item.IsActiveWatchedBefore = true;
                }
                else
                {
                    item.IsActiveWatchedBefore = false;
                }
            }

            return new SearchResponseDto<MovieDto>(response.PageNumber, response.TotalResults, response.TotalPages, result);
        }
    }
}
