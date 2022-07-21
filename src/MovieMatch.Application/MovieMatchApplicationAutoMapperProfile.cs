using AutoMapper;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Movies;
using MovieMatch.Search;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<MovieInfo, MovieDto>();
        CreateMap<Movie, MovieDetailDto>();
    }
}
