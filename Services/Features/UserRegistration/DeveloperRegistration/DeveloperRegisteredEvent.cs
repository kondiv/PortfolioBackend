using Domain.Dto;
using MediatR;

namespace Services.Features.UserRegistration.DeveloperRegistration;

public record DeveloperRegisteredEvent(string UserId, List<SkillDto> Skills) : INotification;