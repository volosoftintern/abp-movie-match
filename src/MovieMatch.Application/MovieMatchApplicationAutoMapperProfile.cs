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
using Volo.CmsKit.Public.Ratings;
using System.Collections.Generic;
using Volo.CmsKit.Comments;
using MovieMatch.Comments;
using Volo.CmsKit.Users;
using DM.MovieApi.MovieDb.People;
using MovieMatch.Posts;
using MovieMatch.Messages;

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
        CreateMap<MovieDetailDto, MovieDto>();
        CreateMap<Movies.Movie, MovieDto>();
        CreateMap<MovieDto, CreateUpdateWatchedBeforeDto>();
        CreateMap<Movies.Movie, CreateUpdateWatchedBeforeDto>();
        CreateMap<Genre, MovieGenreDto>();
        CreateMap<MovieCrewMember, MovieMemeberDto>();
        CreateMap<MovieCastMember, MovieMemeberDto>();
        CreateMap<IdentityUser,IdentityUserDto>();
        CreateMap<Volo.CmsKit.Ratings.Rating,RatingDto>();
        CreateMap<ApiQueryResponse<DM.MovieApi.MovieDb.Movies.Movie>, MovieDto>();
        CreateMap<CommentWithAuthorQueryResultItem, CommentWithStarsDto>();
        CreateMap<CmsUser, CmsUserDto>();
        CreateMap<Comment, CommentDto>();
        CreateMap<Comment, CommentWithStarsDto>();
        CreateMap<Person, PersonDto>();
        CreateMap<Post, PostDto>();
        CreateMap<CreateMessageDto, Message>();
    }
}
