namespace Friends_Data.Helpers.Concerns
{
    public static class AppRole
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly IReadOnlyList<string> All = new[]
        {
            Admin,
            User
        };
    }
}
 