using Microsoft.AspNetCore.Identity;
using Domain.Dto.Authentication;
using Services.Interfaces;
using MediatR;
using Services.Features.UserRegistration.DeveloperRegistration;

namespace Services.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMediator _mediator;

    public AuthenticationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IdentityResult> RegisterDeveloperAsync(
        DeveloperRegistrationDto developerRegistrationDto,
        CancellationToken cancellationToken = default)
    {
        var request = new RegisterDeveloperCommand(developerRegistrationDto);
        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }

    public Task<IdentityResult> RegisterEmployerAsync(
        EmployerRegistrationDto employerRegistrationDto,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}