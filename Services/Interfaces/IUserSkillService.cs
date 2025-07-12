using Domain.Dto;
using Domain.Entities;
using Services.Results;

namespace Services.Interfaces;

public interface IUserSkillService
{
    Task AddAsync(string userId, SkillDto skill, CancellationToken cancellationToken = default);
    Task<Result> AddRangeAsync(string userId, IEnumerable<SkillDto> skills, CancellationToken cancellationToken = default);
}