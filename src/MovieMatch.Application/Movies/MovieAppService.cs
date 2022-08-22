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
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.People;
using DM.MovieApi.MovieDb.Discover;
using Volo.Abp.Domain.Repositories;
using MovieMatch.People;
using Polly;
using System.Threading;
using Polly.Fallback;

namespace MovieMatch.Movies
{
    public class MovieAppService : MovieMatchAppService, IMovieAppService
    {
        private readonly IApiMovieRequest _movieApi;
        private readonly IApiPeopleRequest _peopleApi;
        private readonly IApiDiscoverRequest _discoverApi;
        private readonly IMovieRepository _movieRepository;
        private readonly MovieManager _movieManager;
        private readonly IWatchedBeforeRepository _watchedBeforeRepository;
        private readonly IWatchLaterRepository _watchLaterRepository;
        private readonly List<Movie> _movieList;

        private readonly IRepository<Genres.Genre, int> _genreRepository;
        private readonly IRepository<MovieDetail, int> _movieDetailRepository;
        private readonly IRepository<Director, int> _directorRepository;
        private readonly IRepository<People.Person, int> _personRepository;
        private readonly IRepository<MoviePerson> _moviePersonRepository;

        private AsyncFallbackPolicy<PersonDto> _personPolicy;
        private AsyncFallbackPolicy<MovieDetailDto> _moviePolicy;
        private AsyncFallbackPolicy<MovieCredit> _creditPolicy;
        private AsyncFallbackPolicy<IReadOnlyList<MovieDto>> _moviesPolicy;

        PersonDto _person = null;
        MovieDetailDto _movieDetail = null;
        MovieCredit _movieCredit = null;
        List<MovieDto> _movies = null;

        private int _personId = 0;
        public MovieAppService(
            IMovieRepository movieRepository,
            MovieManager movieManager,
            IWatchedBeforeRepository watchedBeforeRepository,
            IWatchLaterRepository watchLaterRepository,
            IRepository<MovieDetail, int> movieDetailRepository,
            IRepository<Director, int> directorRepository,
            IRepository<People.Person, int> personRepository,
            IRepository<Genres.Genre, int> genreRepository,
            IRepository<MoviePerson> moviePersonRepository
            )
        {
            _movieList = new List<Movie>();
            _movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
            _peopleApi = MovieDbFactory.Create<IApiPeopleRequest>().Value;
            _discoverApi = MovieDbFactory.Create<IApiDiscoverRequest>().Value;
            _movieRepository = movieRepository;
            _movieManager = movieManager;

            _watchedBeforeRepository = watchedBeforeRepository;
            _watchLaterRepository = watchLaterRepository;
            _movieDetailRepository = movieDetailRepository;
            _directorRepository = directorRepository;
            _personRepository = personRepository;
            _genreRepository = genreRepository;
            _moviePersonRepository = moviePersonRepository;


            _moviePolicy = Policy<MovieDetailDto>.Handle<Exception>().FallbackAsync(_movieDetail);
            _creditPolicy = Policy<MovieCredit>.Handle<Exception>().FallbackAsync(_movieCredit);
            _personPolicy = Policy<PersonDto>.Handle<Exception>().FallbackAsync(_person);
            _moviesPolicy = Policy<IReadOnlyList<MovieDto>>.Handle<Exception>().FallbackAsync(_movies);
        }

        public async Task<MovieDto> GetFromDbAsync(int id)
        {
            var movie = await _movieRepository.GetAsync(id);

            var resp = ObjectMapper.Map<Movie, MovieDto>(movie);

            resp.IsActiveWatchedBefore = (await _watchedBeforeRepository.GetQueryableAsync()).Any(x => x.MovieId == resp.Id && x.UserId == CurrentUser.Id);
            resp.IsActiveWatchLater = (await _watchLaterRepository.GetQueryableAsync()).Any(x => x.MovieId == resp.Id && x.UserId == CurrentUser.Id);

            return resp;
        }
        public async Task<MovieDto> GetMovieAsync(int id)
        {
            var movie = await GetAsync(id);

            return ObjectMapper.Map<MovieDetailDto, MovieDto>(movie);
        }
        public async Task<MovieDetailDto> GetAsync(int id)//Implement polly
        {

            var isMoveExist = (await _movieDetailRepository.GetQueryableAsync()).Where(x => x.Id == id).Count() > 0;

            if (isMoveExist)
            {

                var movie = (await _movieDetailRepository.WithDetailsAsync(x => x.Genres, x => x.Stars)).FirstOrDefault(x => x.Id == id);

                _movieDetail = ObjectMapper.Map<MovieDetail, MovieDetailDto>(movie);
                var dir = await _directorRepository.GetAsync(movie.DirectorId);
                _movieDetail.Director = ObjectMapper.Map<Director, PersonDto>(dir);
                var movieStars = (await _personRepository.GetListAsync()).Where(x => movie.Stars.ToList().Any(p => p.PersonId == x.Id)).ToArray();
                var movieGenres = (await _genreRepository.GetListAsync()).Where(x => movie.Genres.ToList().Any(p => p.GenreId == x.Id)).ToArray();
                _movieDetail.Stars = ObjectMapper.Map<IEnumerable<People.Person>, IEnumerable<PersonDto>>(movieStars);
                _movieDetail.Genres = ObjectMapper.Map<IEnumerable<Genres.Genre>, IEnumerable<GenreDto>>(movieGenres);
                _movieCredit = new MovieCredit()
                {
                    CrewMembers = new List<MovieCrewMember>() { new MovieCrewMember() { PersonId = dir.Id, Job = "Director", Name = dir.Name, ProfilePath = dir.ProfilePath } }
                    ,
                    CastMembers = ObjectMapper.Map<IEnumerable<PersonDto>, List<MovieCastMember>>(_movieDetail.Stars.ToList())
                };

                _moviePolicy = Policy<MovieDetailDto>.Handle<Exception>().FallbackAsync(_movieDetail);
                _creditPolicy = Policy<MovieCredit>.Handle<Exception>().FallbackAsync(_movieCredit);

            }

            _movieDetail = await _moviePolicy.ExecuteAsync(async () =>
            {
                var response = await _movieApi.FindByIdAsync(id);
                return ObjectMapper.Map<DM.MovieApi.MovieDb.Movies.Movie, MovieDetailDto>(response.Item);
            });

            _movieCredit = await _creditPolicy.ExecuteAsync(async () =>
            {

                var resp = await _movieApi.GetCreditsAsync(id);
                return resp.Item;
            });

            var director = _movieCredit.CrewMembers.FirstOrDefault((c) => c.Job == "Director");
            var stars = _movieCredit.CastMembers.Take(3);//stars

            _movieDetail.Director = ObjectMapper.Map<MovieCrewMember, PersonDto>(director);
            _movieDetail.Stars = ObjectMapper.Map<IEnumerable<MovieCastMember>, IEnumerable<PersonDto>>(stars);

            _movieDetail.IsActiveWatchedBefore = (await _watchedBeforeRepository.GetQueryableAsync()).Any(x => x.MovieId == _movieDetail.Id && x.UserId == CurrentUser.Id);
            _movieDetail.IsActiveWatchLater = (await _watchLaterRepository.GetQueryableAsync()).Any(x => x.MovieId == _movieDetail.Id && x.UserId == CurrentUser.Id);

            await GetPersonAsync(director.PersonId, true);

            foreach (var item in _movieDetail.Stars)
            {
                await GetPersonAsync(item.Id, false);
            }

            if ((await _movieDetailRepository.GetQueryableAsync()).Where(x => x.Id == id).Count() == 0)
            {
                var savedMovie = new MovieDetail(_movieDetail.Id, _movieDetail.Title, _movieDetail.PosterPath, _movieDetail.Overview, _movieDetail.ReleaseDate, _movieDetail.VoteAverage, _movieDetail.ImdbId, _movieDetail.Director.Id);

                foreach (var item in _movieDetail.Genres)
                {
                    savedMovie.Genres.Add(new MovieGenre(movieId: savedMovie.Id, genreId: item.Id));
                }

                foreach (var item in _movieDetail.Stars)
                {
                    savedMovie.Stars.Add(new MoviePerson(savedMovie.Id, item.Id));
                }

                await _movieDetailRepository.InsertAsync(savedMovie, true);
            }

            return _movieDetail;
        }
        public async Task<MovieDto> CreateAsync(CreateMovieDto input)
        {
            var movie = _movieManager.Create(input.Id, input.Title, input.PosterPath, input.Overview);
            try
            {
                await _movieRepository.InsertAsync(movie, true);
            }
            catch (Exception ex)
            {
                throw new BusinessException(null, ex.Message);
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
            var moviesWatchedBefore = await _watchedBeforeRepository.GetListAsync(x => x.UserId == CurrentUser.Id);
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
                if (addMovie != null)
                {
                    _movieList.Add(addMovie);
                }

            }

            moviesWatchLater = moviesWatchLater
             .Skip(input.SkipCount)
             .Take(input.MaxResultCount).
               ToList();

            //Convert the query result to a list of movieDto objects
            var movieDtos = ObjectMapper.Map<List<Movie>, List<MovieDto>>(_movieList);


            //Get the total count with another query

            return new PagedResultDto<MovieDto>(
             totalCount,
             movieDtos
         );

        }

        public async Task<PersonDto> GetPersonAsync(int personId, bool isDirector = false)//Implemented Polly
        {

            _personId = personId;

            if (isDirector && (await _directorRepository.GetQueryableAsync()).Where(x => x.Id == personId).Count() > 0)
            {
                var dir = await _directorRepository.GetAsync(personId);
                _person = new PersonDto() { Id = dir.Id, Biography = dir.Biography, DeathDay = dir.DeathDay, BirthDay = dir.BirthDay, Name = dir.Name, ProfilePath = dir.ProfilePath };
                _personPolicy = Policy<PersonDto>.Handle<Exception>().FallbackAsync(_person);
            }

            if (!isDirector && (await _personRepository.GetQueryableAsync()).Where(x => x.Id == personId).Count() > 0)
            {
                var actor = await _personRepository.GetAsync(personId);
                _person = new PersonDto() { Id = actor.Id, Biography = actor.Biography, DeathDay = actor.DeathDay, BirthDay = actor.BirthDay, Name = actor.Name, ProfilePath = actor.ProfilePath };
                _personPolicy = Policy<PersonDto>.Handle<Exception>().FallbackAsync(_person);
            }

            _person = await _personPolicy.ExecuteAsync(async () =>
            {
                var response = await _peopleApi.FindByIdAsync(_personId);
                return ObjectMapper.Map<DM.MovieApi.MovieDb.People.Person, PersonDto>(response.Item);
            });

            if (isDirector && (await _directorRepository.GetQueryableAsync()).Where(x => x.Id == personId).Count() == 0)
            {
                await _directorRepository.InsertAsync(new Director(personId, _person.Name, _person.ProfilePath, _person.BirthDay, _person.DeathDay, _person.Biography), true);
            }

            if (!isDirector && (await _personRepository.GetQueryableAsync()).Where(x => x.Id == personId).Count() == 0)
            {
                await _personRepository.InsertAsync(new People.Person(personId, _person.Name, _person.ProfilePath, _person.BirthDay, _person.DeathDay, _person.Biography), true);
            }

            return _person;

        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _movieRepository.AnyAsync(id);
        }

        public async Task<PagedResultDto<MovieDto>> GetPersonMoviesAsync(PersonMovieRequestDto input)
        {
            int totalCount = 0;

            if (input.IsDirector && (await _moviePersonRepository.GetQueryableAsync()).Where(x => x.PersonId == input.PersonId).Count() > 0)
            {
                
                var movieIds = (await _moviePersonRepository.GetQueryableAsync())
                    .Where(x => x.PersonId == input.PersonId)
                    .Select(x => x.MovieDetailId);
                totalCount = movieIds.Count();

                var dbMovies = (await _movieRepository.GetQueryableAsync()).Where(x => movieIds.Any(m => m == x.Id))
                    .Skip(input.SkipCount)
                    .Take(input.MaxCount)
                    .ToList();

                var movies = ObjectMapper.Map<IReadOnlyList<Movie>, IReadOnlyList<MovieDto>>(dbMovies);
                _moviesPolicy = Policy<IReadOnlyList<MovieDto>>.Handle<Exception>().FallbackAsync(movies);
            }
            else if (!input.IsDirector && (await _movieDetailRepository.GetQueryableAsync()).Where(x => x.DirectorId == input.PersonId).Count() > 0)
            {
                var movieIds = (await _movieDetailRepository.GetQueryableAsync())
                    .Where(x => x.DirectorId == input.PersonId)
                    .Select(x => x.Id);

                totalCount = movieIds.Count();
                var dbMovies = (await _movieRepository.GetQueryableAsync())
                    .Where(x => movieIds.Any(m => m == x.Id))
                    .Skip(input.SkipCount)
                    .Take(input.MaxCount).ToList();

                var movies = ObjectMapper.Map<IReadOnlyList<Movie>, IReadOnlyList<MovieDto>>(dbMovies);
                _moviesPolicy = Policy<IReadOnlyList<MovieDto>>.Handle<Exception>().FallbackAsync(movies);
            }

            var personMovies = await _moviesPolicy.ExecuteAsync(async () =>
            {
                var paramBuilder = new DiscoverMovieParameterBuilder();
                if (input.IsDirector)
                {
                    paramBuilder.WithCrew(input.PersonId);
                }
                else
                {
                    paramBuilder.WithCast(input.PersonId);
                }

                var response = await _discoverApi.DiscoverMoviesAsync(paramBuilder,input.PageNumber);
                totalCount = response.TotalResults;
                return ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);
            });

            return new PagedResultDto<MovieDto>(totalCount,personMovies);
        }
        public async Task<IReadOnlyList<MovieDto>> GetSimilarMoviesAsync(int id)
        {
            var response = await _movieApi.GetSimilarAsync(id);
            var result = ObjectMapper.Map<IReadOnlyList<MovieInfo>, IReadOnlyList<MovieDto>>(response.Results);
            return result;


        }
    }
}
