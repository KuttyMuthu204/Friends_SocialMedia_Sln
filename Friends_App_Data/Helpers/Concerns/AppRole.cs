using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends_App_Data.Helpers.Concerns
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
 