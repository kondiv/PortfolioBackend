using Domain.Dto;
using Services.Results;

namespace Services.Strategies.Registration;

public interface IRegistrationStrategy
{
    Task<RegistrationResult> Register(CancellationToken cancellationToken = default);
}