using MediatR;
using Services.Interfaces;

namespace Services.Features.UserRegistration.DeveloperRegistration;

public class AddSkillsHandler : INotificationHandler<DeveloperRegisteredEvent>
{
    private readonly IUserSkillService _userSkillService;

    public AddSkillsHandler(IUserSkillService userSkillService)
    {
        _userSkillService = userSkillService;
    }
    
    public async Task Handle(DeveloperRegisteredEvent notification, CancellationToken cancellationToken)
    {
        await _userSkillService.AddRangeAsync(notification.UserId, notification.Skills, cancellationToken);
    }
}