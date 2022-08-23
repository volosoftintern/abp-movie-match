using DM.MovieApi;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using Polly;
using Polly.Fallback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.ObjectMapping;
using Movie = MovieMatch.Movies.Movie;

namespace MovieMatch.Search
{
    public class SearchService : MovieMatchAppService, ISearchService
    {
        private readonly IWatchLaterRepository _watchLaterRepository;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly IApiMovieRequest _movieApi;
        private readonly IMovieRepository _movieRepository;
        private readonly MovieManager _movieManager;
        private AsyncFallbackPolicy<IReadOnlyList<MovieDto>> _moviesPolicy;

        List<MovieDto> _movies;


        public SearchService(IWatchLaterRepository watchLaterRepository,
            IWatchedBeforeRepository watchedBeforeRepository, MovieManager movieManager,
            IMovieRepository movieRepository)
        {
            _watchedBeforeRepository = watchedBeforeRepository;
            _watchLaterRepository = watchLaterRepository;
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
            _movieManager = movieManager;
            _moviesPolicy= Policy<IReadOnlyList<MovieDto>>.Handle<Exception>().FallbackAsync(_movies);
            _movieRepository = movieRepository;
        }


        public async Task<SearchResponseDto<MovieDto>> GetMovies(SearchMovieDto input)//Implement Polly
        {
            IReadOnlyList<MovieDto> result=new List<MovieDto>();
            ApiSearchResponse<MovieInfo> response;
            int pageNumber = input.CurrentPage;
            int totalResults = 0;
            int totalPages = 0;


            if (string.IsNullOrEmpty(input.Name))//popular
            {
                int maxResult = 20;
                //
                var query = (await _movieRepository.GetQueryableAsync());
                totalResults = query.Count();

                    query.OrderByDescending(x => x.Id).Skip((input.CurrentPage - 1) * maxResult).Take(maxResult);
                
                var queryresult = await AsyncExecuter.ToListAsync(query);
                result = ObjectMapper.Map<List<Movie>, List<MovieDto>>(queryresult);
                totalPages = totalResults / maxResult;
                _moviesPolicy= Policy<IReadOnlyList<MovieDto>>.Handle<Exception>().FallbackAsync(result);
                await _moviesPolicy.ExecuteAsync(async () =>
                {
                     response = await _movieApi.GetPopularAsync(input.CurrentPage);
                    totalResults = response.TotalResults;
                    pageNumber = response.PageNumber;
                    totalPages = response.TotalPages;
                    result = ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);
                    return result;
                });


                
            }
            else//
            {
                var query = (await _movieRepository.GetQueryableAsync());
                int maxResult = 20;

                query = query.Where(x => x.Title.Contains(input.Name));
                totalResults = query.Count();
                query.OrderByDescending(x => x.Id).Skip((input.CurrentPage - 1) * maxResult).Take(maxResult);
                var queryresult = await AsyncExecuter.ToListAsync(query);
                result = ObjectMapper.Map<List<Movie>, List<MovieDto>>(queryresult);
                _moviesPolicy = Policy<IReadOnlyList<MovieDto>>.Handle<Exception>().FallbackAsync(result);
                await _moviesPolicy.ExecuteAsync(async () =>
                {
                    response = await _movieApi.SearchByTitleAsync(input.Name, input.CurrentPage);
                    totalResults = response.TotalResults;
                    pageNumber = response.PageNumber;
                    totalPages = response.TotalPages;
                    result = ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);
                    return result;
                });
                //
            }

            await BackupMoviesAsync(result);

            foreach (var item in result)
            {
                item.IsActiveWatchLater = (await _watchLaterRepository.GetQueryableAsync()).Any(x => x.UserId == CurrentUser.Id && x.MovieId==item.Id);
                item.IsActiveWatchedBefore = (await _watchedBeforeRepository.GetQueryableAsync()).Any(x => x.UserId == CurrentUser.Id && x.MovieId == item.Id);
            }

            return new SearchResponseDto<MovieDto>(pageNumber, totalResults,totalPages,result);

        }

        private async Task BackupMoviesAsync(IReadOnlyList<MovieDto> movies)
        {
            foreach (var item in movies)
            {
                if(!(await _movieRepository.AnyAsync(item.Id)))
                {
                    var movie = _movieManager.Create(item.Id, item.Title, item.PosterPath, item.Overview);
                    try
                    {
                        await _movieRepository.InsertAsync(movie, true);
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException(null, ex.Message);
                    }
                }
            }
        }


    }
}
