using Domain.Dto;
using Domain.Entities;
using Mapster;

namespace Services.Mapper;

public class UserDtoMapper
{
    public static void Configure()
    {
        TypeAdapterConfig<ApplicationUser, UserDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserName, src => src.UserName)
            .Map(dest => dest.Email, src => src.Email);
    }
}