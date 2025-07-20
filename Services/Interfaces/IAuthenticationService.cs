using Domain.Dto;
using Domain.Dto.Authentication;
using Services.Results;

namespace Services.Interfaces;

public interface IAuthenticationService
{
    Task<RegistrationResult> RegisterDeveloperAsync(DeveloperRegistrationDto developerRegistrationDto, CancellationToken cancellationToken = default);
    Task<RegistrationResult> RegisterUserAsync(EmployerRegistrationDto employerRegistrationDto, CancellationToken cancellationToken = default);
}