using AutoMapper;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Search;
using DM.MovieApi.MovieDb.Genres;
using MovieMatch.UserConnections;
using System;
using Volo.Abp.Identity;
using DM.MovieApi.MovieDb.People;
using MovieMatch.Posts;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<UserConnection, UserConnectionDto>();
        CreateMap<UserConnection,IdentityUserDto >();
        CreateMap<IdentityUser,UserConnectionDto >();
        CreateMap<CreateUpdateUserConnectionDto, UserConnection>();
        //CreateMap<Guid,FollowerDto>();
        CreateMap<WatchedBefore, WatchedBeforeDto>();
        CreateMap<WatchLater, WatchLaterDto>();
        CreateMap<CreateUpdateWatchLaterDto, WatchLater>();
        CreateMap<CreateUpdateWatchedBeforeDto, WatchedBefore>();
        CreateMap<MovieInfo, MovieDto>();
        CreateMap<DM.MovieApi.MovieDb.Movies.Movie, MovieDetailDto>();
        CreateMap<DM.MovieApi.MovieDb.Movies.Movie, MovieDto>();
        CreateMap<Movies.Movie, MovieDto>();
        CreateMap<MovieDto, CreateUpdateWatchedBeforeDto>();
        CreateMap<Movies.Movie, CreateUpdateWatchedBeforeDto>();
        CreateMap<Genre, MovieGenreDto>();
        CreateMap<MovieCrewMember, MovieMemeberDto>();
        CreateMap<MovieCastMember, MovieMemeberDto>();
        CreateMap<Person, PersonDto>();
        CreateMap<Post, PostDto>();
    }
}
