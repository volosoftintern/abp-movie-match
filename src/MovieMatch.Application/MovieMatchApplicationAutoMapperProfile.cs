using AutoMapper;
using MovieMatch.UserConnections;
using System;
using Volo.Abp.Identity;

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
       
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
