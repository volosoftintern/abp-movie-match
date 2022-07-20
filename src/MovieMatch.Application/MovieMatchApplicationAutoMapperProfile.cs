using AutoMapper;
using DM.MovieApi.MovieDb.Movies;
using MovieMatch.Movies;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<MovieInfo, MovieDto>();
        CreateMap<Movie, MovieDetailDto>();
    }
}
