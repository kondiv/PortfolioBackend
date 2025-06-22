namespace Models.Entities
{
    public class UserSkill
    {
        public int UserSkillId { get; set; }

        public string UserId { get; set; } = default!;
        public int SkillId { get; set; }

        public int Proficiency{ get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Skill Skill { get; set; } = null!;
    }
}
