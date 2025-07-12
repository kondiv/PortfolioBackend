using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Services.Features.UserRegistration;

public record RegisterUserCommand(
    DeveloperRegistrationDto DeveloperRegistrationDto,
    CancellationToken CancellationToken = default) 
    : IRequest<IdentityResult>;