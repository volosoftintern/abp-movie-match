using AutoMapper;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.People;
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
        CreateMap<WatchedBefore, WatchedBeforeDto>();
        CreateMap<WatchLater, WatchLaterDto>();
        CreateMap<CreateUpdateWatchLaterDto, WatchLater>();
        CreateMap<CreateUpdateWatchedBeforeDto, WatchedBefore>();
        CreateMap<CmsUser, MyCmsUserDto>();
       
        //Movie
        CreateMap<MovieInfo, MovieDto>();
        
        CreateMap<MovieDetail, MovieDetailDto>()
            .ForMember(s=>s.Genres,t=>t.Ignore())
            .ForMember(s=>s.Stars,t=>t.Ignore());
        CreateMap<DM.MovieApi.MovieDb.Movies.Movie, MovieDetailDto>();
        CreateMap<DM.MovieApi.MovieDb.Movies.Movie, MovieDto>();
        CreateMap<MovieDetailDto, MovieDto>();
        CreateMap<Movies.Movie, MovieDto>();
        CreateMap<MovieDto, CreateUpdateWatchedBeforeDto>();
        CreateMap<Movies.Movie, CreateUpdateWatchedBeforeDto>();
        CreateMap<ApiQueryResponse<DM.MovieApi.MovieDb.Movies.Movie>, MovieDto>();
        
        //Genre
        CreateMap<Genre, GenreDto>();
        CreateMap<Genres.Genre, GenreDto>();
        
        //IdentityUser
        CreateMap<IdentityUser,IdentityUserDto>();

        //Rating
        CreateMap<Volo.CmsKit.Ratings.Rating,RatingDto>();
        

        //Comment
        CreateMap<CommentWithAuthorQueryResultItem, CommentWithStarsDto>();
        CreateMap<CmsUser, CmsUserDto>();
        CreateMap<Comment, CommentDto>();
        CreateMap<Comment, CommentWithStarsDto>();

        //Person
        CreateMap<PersonDto, MovieCastMember>().ForMember(s => s.PersonId, opt => opt.MapFrom(t => t.Id));
        CreateMap<People.Person, PersonDto>();
        CreateMap<DM.MovieApi.MovieDb.People.Person, PersonDto>();
        CreateMap<DM.MovieApi.MovieDb.People.Person, People.Person>();
        CreateMap<Director, DirectorDto>();
        CreateMap<Director, PersonDto>();
        CreateMap<MovieCrewMember, PersonDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonId));
        CreateMap<MovieCastMember, PersonDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonId));

        //Post
        CreateMap<Post, PostDto>();
        CreateMap<CreateMessageDto, Message>();
    }
}
