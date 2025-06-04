namespace Models.SeedData
{
    public static class DefaultRoles
    {
        public const string Guest = "Guest";
        public const string Admin = "Admin";
        public const string Developer = "Developer";
        public const string Employer = "Employer";

        public static List<string> All() => [Guest, Admin, Developer, Employer];
    }
}
