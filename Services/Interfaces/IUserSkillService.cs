using Domain.Dto;
using Domain.Entities;
using Services.Results;

namespace Services.Interfaces;

public interface IUserSkillService
{
    Task<Result> AddRangeAsync(string userId, List<SkillDto> skills, CancellationToken cancellationToken = default);
}