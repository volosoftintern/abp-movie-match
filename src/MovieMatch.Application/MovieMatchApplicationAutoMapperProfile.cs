using AutoMapper;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;

namespace MovieMatch;

public class MovieMatchApplicationAutoMapperProfile : Profile
{
    public MovieMatchApplicationAutoMapperProfile()
    {
        CreateMap<WatchedBefore, WatchedBeforeDto>();
        CreateMap<WatchLater, WatchLaterDto>();
        CreateMap<CreateUpdateWatchLaterDto, WatchLater>();
        CreateMap<CreateUpdateWatchedBeforeDto, WatchedBefore>();
    }
}
