namespace Domain.Entities
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserSkill>? UserSkills { get; set; } 
    }
}
