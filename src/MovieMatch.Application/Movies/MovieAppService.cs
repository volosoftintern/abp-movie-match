using Volo.Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using DM.MovieApi.MovieDb.Genres;


namespace MovieMatch.Movies
{
    public class MovieAppService : MovieMatchAppService, IMovieAppService
    {
        private readonly IApiMovieRequest _movieApi;
        private readonly IMovieRepository _movieRepository;
        private readonly MovieManager _movieManager;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly IWatchLaterRepository _watchLaterRepository;
        private readonly List<Movie> _movieList;

        public MovieAppService(
            IMovieRepository movieRepository,
            MovieManager movieManager,
            IWatchedBeforeRepository watchedBeforeRepository,
            IWatchLaterRepository watchLaterRepository
            )
        {
            _movieList= new List<Movie>();
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
            _movieRepository = movieRepository;
            _movieManager = movieManager;
            _watchedBeforeRepository = watchedBeforeRepository;
            _watchLaterRepository = watchLaterRepository;
        }

        public async Task<MovieDetailDto> GetAsync(int id)
        {
            var response = await _movieApi.FindByIdAsync(id);
            
            var credits= await _movieApi.GetCreditsAsync(id);
            var director = credits.Item.CrewMembers.FirstOrDefault((c) => c.Job == "Director");
            var stars = credits.Item.CastMembers.Take(3);//stars
            
            var movieDetail=ObjectMapper.Map<DM.MovieApi.MovieDb.Movies.Movie, MovieDetailDto>(response.Item);
            
            movieDetail.Director = ObjectMapper.Map<MovieCrewMember,MovieMemeberDto>(director);
            movieDetail.Stars= ObjectMapper.Map<IEnumerable<MovieCastMember>, IEnumerable<MovieMemeberDto>>(stars);
            //movieDetail.Genres= ObjectMapper.Map<IReadOnlyList<Genre>, IReadOnlyList<MovieGenreDto>>(response.Item.Genres);


            return movieDetail;
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto input)
        {
            var movie = _movieManager.Create(input.Id, input.Title, input.PosterPath, input.Overview,input.IsActive);
            try
            {
                await _movieRepository.InsertAsync(movie,true);
            }
            catch (Exception ex)
            {
                throw new  BusinessException(null,ex.Message);
            }
            
            return ObjectMapper.Map<Movie, MovieDto>(movie);
        }
        public async Task DeleteMoviesWatchedBeforeAsync(int id)
        {
            var movie = await _watchedBeforeRepository.FindByIdAsync(id);
            await _watchedBeforeRepository.DeleteAsync(movie);
        }
        public async Task DeleteMoviesWatchLaterAsync(int id)
        {
            var movie = await _watchLaterRepository.FindByIdAsync(id);
            await _watchLaterRepository.DeleteAsync(movie);
        }

        public async Task<PagedResultDto<MovieDto>> GetWatchedBeforeListAsync(PagedAndSortedResultRequestDto input)
        {
            var moviesWatchedBefore= await _watchedBeforeRepository.GetListAsync(x => x.UserId == CurrentUser.Id);
            var queryable = await _movieRepository.GetQueryableAsync();

            var totalCount = moviesWatchedBefore.Count();

            //var queryResult = await AsyncExecuter.ToListAsync(moviesWatchedBefore);

            foreach (var item in moviesWatchedBefore)
            {
                var addMovie = queryable.FirstOrDefault(x => x.Id == item.MovieId);
                if (addMovie != null)
                {
                    _movieList.Add(addMovie);
                }

            }

            moviesWatchedBefore = moviesWatchedBefore
             .Skip(input.SkipCount)
             .Take(input.MaxResultCount).ToList();
            //Convert the query result to a list of movieDto objects
            var movieDtos = ObjectMapper.Map<List<Movie>, List<MovieDto>>(_movieList);


            //Get the total count with another query

            return new PagedResultDto<MovieDto>(
             totalCount,
             movieDtos
         );

        }
        public async Task<PagedResultDto<MovieDto>> GetWatchLaterListAsync(PagedAndSortedResultRequestDto input)
        {
            var moviesWatchLater = await _watchLaterRepository.GetListAsync(x => x.UserId == CurrentUser.Id);
            var queryable = await _movieRepository.GetQueryableAsync();

            var totalCount = moviesWatchLater.Count();


          
           // var queryResult = await AsyncExecuter.ToListAsync(moviesWatchLater);

            foreach (var item in moviesWatchLater)
            {
                var addMovie = queryable.FirstOrDefault(x => x.Id == item.MovieId);
                if(addMovie != null)
                {
                    _movieList.Add(addMovie);
                }
                
            }

            moviesWatchLater = moviesWatchLater
             .Skip(input.SkipCount)
             .Take(input.MaxResultCount).
               ToList();

            //Convert the query result to a list of movieDto objects
            var movieDtos =ObjectMapper.Map<List<Movie>, List<MovieDto>>(_movieList);
            

            //Get the total count with another query

            return new PagedResultDto<MovieDto>(
             totalCount,
             movieDtos
         );

        }



        public async Task<MovieDto> GetMovieAsync(int id)
        {
            var api= await _movieApi.FindByIdAsync(id);
            var findMovie = api.Item;
            var movie=ObjectMapper.Map<DM.MovieApi.MovieDb.Movies.Movie, MovieDto>(findMovie);

            var userMovieWL = await _watchLaterRepository.FindByIdAsync(id);
            if(userMovieWL != null)    movie.IsActiveWatchLater = true;
            else    movie.IsActiveWatchLater = false;

            var userMovieWB = await _watchedBeforeRepository.FindByIdAsync(id);
            if (userMovieWB != null) movie.IsActiveWatchedBefore = true;
            else movie.IsActiveWatchedBefore = false;

            return movie;
        }
    }
}
