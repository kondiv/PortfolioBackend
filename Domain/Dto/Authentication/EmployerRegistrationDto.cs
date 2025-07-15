namespace Domain.Dto.Authentication;

public record EmployerRegistrationDto(
    string FullName,
    string Email,
    string Password,
    string AvatarUrl
    ) : IAuthenticationDto;