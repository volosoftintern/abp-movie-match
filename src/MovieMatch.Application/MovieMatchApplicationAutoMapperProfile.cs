using AutoMapper;
using DM.MovieApi.MovieDb.Movies;
using IMDbApiLib.Models;
using MovieMatch.Movies;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<MovieInfo, MovieDto>();
    }
}
