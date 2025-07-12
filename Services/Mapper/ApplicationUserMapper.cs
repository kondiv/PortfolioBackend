using Mapster;
using Domain.Dto;
using Domain.Dto.Authentication;
using Domain.Entities;

namespace Services.Mapper;

public class ApplicationUserMapper
{
    public static void Configure()
    {
        TypeAdapterConfig<IAuthenticationDto, ApplicationUser>
            .NewConfig()
            .Map(dest => dest.UserName, src => src.Email)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.NormalizedEmail!)
            .Ignore(dest => dest.NormalizedUserName!)
            .Ignore(dest => dest.PasswordHash!);

        TypeAdapterConfig<DeveloperRegistrationDto, ApplicationUser>
            .NewConfig()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.DeveloperLevel, src => src.DeveloperLevel)
            .Map(dest => dest.AvatarUrl, src => src.AvatarUrl)
            .Map(dest => dest.ExperienceYears, src => src.ExperienceYears)
            .Map(dest => dest.Bio, src => src.Bio)
            .Ignore(dest => dest.UserSkills);

        TypeAdapterConfig<EmployerRegistrationDto, ApplicationUser>
            .NewConfig()
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.AvatarUrl, src => src.AvatarUrl);
    }
}