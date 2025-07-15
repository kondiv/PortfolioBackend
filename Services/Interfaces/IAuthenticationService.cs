using Microsoft.AspNetCore.Identity;
using Domain.Dto.Authentication;

namespace Services.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterDeveloperAsync(DeveloperRegistrationDto developerRegistrationDto, CancellationToken cancellationToken = default);
    Task<IdentityResult> RegisterEmployerAsync(EmployerRegistrationDto employerRegistrationDto, CancellationToken cancellationToken = default);
}