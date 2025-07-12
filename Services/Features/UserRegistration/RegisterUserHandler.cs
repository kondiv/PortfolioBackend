using Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Features.UserRegistration.DeveloperRegistration;

namespace Services.Features.UserRegistration;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RegisterUserHandler(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<ApplicationUser>(request.DeveloperRegistrationDto);
        var result = await _userManager.CreateAsync(user, request.DeveloperRegistrationDto.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        await _mediator.Publish(new DeveloperRegisteredEvent(user.Id, request.DeveloperRegistrationDto.Skills), cancellationToken);
        
        return result;
    }
}