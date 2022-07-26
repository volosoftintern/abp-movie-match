using AutoMapper;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Search;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<WatchedBefore, WatchedBeforeDto>();
        CreateMap<WatchLater, WatchLaterDto>();
        CreateMap<CreateUpdateWatchLaterDto, WatchLater>();
        CreateMap<CreateUpdateWatchedBeforeDto, WatchedBefore>();
        CreateMap<MovieInfo, MovieDto>();
        CreateMap<DM.MovieApi.MovieDb.Movies.Movie, MovieDetailDto>();
        CreateMap<Movies.Movie, MovieDto>();
        CreateMap<MovieDto, CreateUpdateWatchedBeforeDto>();
        CreateMap<Movies.Movie, CreateUpdateWatchedBeforeDto>();
        
    }
}
