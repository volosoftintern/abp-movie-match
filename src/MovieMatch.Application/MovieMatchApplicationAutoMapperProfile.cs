using AutoMapper;
using IMDbApiLib.Models;
using MovieMatch.Movies;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<SearchResult, MovieDto>();
    }
}
