using Domain.Dto.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Services.Features.UserRegistration.DeveloperRegistration;

public record RegisterDeveloperCommand(
    DeveloperRegistrationDto DeveloperRegistrationDto)
    : IRequest<IdentityResult>;