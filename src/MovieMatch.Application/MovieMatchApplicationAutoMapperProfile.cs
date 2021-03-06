using AutoMapper;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Search;
using DM.MovieApi.MovieDb.Genres;

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
        CreateMap<Genre, MovieGenreDto>();
        CreateMap<MovieCrewMember, MovieMemeberDto>();
        CreateMap<MovieCastMember, MovieMemeberDto>();
        CreateMap<Movie, MovieDetailDto>();

    }
}
