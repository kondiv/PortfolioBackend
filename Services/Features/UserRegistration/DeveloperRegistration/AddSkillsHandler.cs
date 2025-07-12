using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;

namespace Services.Features.UserRegistration.DeveloperRegistration;

public class AddSkillsHandler : INotificationHandler<DeveloperRegisteredEvent>
{
    private readonly IUserSkillService _userSkillService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AddSkillsHandler(IUserSkillService userSkillService, UserManager<ApplicationUser> userManager)
    {
        _userSkillService = userSkillService;
        _userManager = userManager;
    }
    
    public async Task Handle(DeveloperRegisteredEvent notification, CancellationToken cancellationToken)
    {
        await _userSkillService.AddRangeAsync(notification.UserId, notification.Skills, cancellationToken);
    }
}