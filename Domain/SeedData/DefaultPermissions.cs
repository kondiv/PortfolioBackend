namespace Domain.SeedData
{
    public static class DefaultPermissions
    {
        public const string ViewProjects = "ViewProjects";
        public const string InteractWithProjects = "InteractWithProjects";
        public const string InteractWithDevelopers = "InteractWithDevelopers";
        public const string InteractWithEmployers = "InteractWithEmployers";
        public const string ManageUsers = "ManageUsers";

        public static List<string> All() =>
            [ViewProjects, InteractWithProjects, InteractWithDevelopers, InteractWithEmployers, ManageUsers];
    }
}
