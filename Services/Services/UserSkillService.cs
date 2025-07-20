using Data.Interfaces;
using Domain.Dto;
using Domain.Entities;
using Services.Interfaces;
using Services.Results;

namespace Services.Services
{
    public class UserSkillService : IUserSkillService
    {
        private readonly ISkillValidator _skillValidator;
        private readonly IUserSkillRepository _userSkillRepository;

        public UserSkillService(
            ISkillValidator skillValidator,
            IUserSkillRepository userSkillRepository)
        {
            _skillValidator = skillValidator;
            _userSkillRepository = userSkillRepository;
        }

        public async Task<Result> AddRangeAsync(string userId, List<SkillDto> skills, CancellationToken cancellationToken = default)
        {
            var skillsValid = await _skillValidator.ValidateSkillsAsync(skills, cancellationToken);

            if(!skillsValid)
            {
                return Result.Failed(new Error() { Description = "Invalid skills provided" });
            }

            var userSkills = CreateUserSkills(userId, skills);

            await _userSkillRepository.AddRangeAsync(userSkills, cancellationToken);

            return Result.Success;
        }

        private static List<UserSkill> CreateUserSkills(string userId, List<SkillDto> skills)
        {
            return skills.Select(s =>
            new UserSkill()
            { 
                UserId = userId,
                SkillId = s.SkillId,
                Proficiency = s.Proficiency.Value,
            }).ToList();
        }
    }
}
