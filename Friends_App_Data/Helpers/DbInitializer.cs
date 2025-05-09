using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Friends_App_Data.Helpers.Concerns;
using Microsoft.AspNetCore.Identity;

namespace Friends_App_Data.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedUsersAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //Roles
            if (!roleManager.Roles.Any())
            {
                foreach (var role in AppRole.All)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(role));
                    }
                }
            }

            //Users with Roles
            if (!userManager.Users.Any(n => !string.IsNullOrEmpty(n.Email)))
            {
                var userPassword = "Muthuraman@1234";

                var newUser = new User()
                {
                    UserName = "muthuraman",
                    Email = "smuthuram47@gmail.com",
                    FullName = "Muthuraman S",
                    ProfilePictureUrl = "~/images/profilePictures/Muthuraman.jpg",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, userPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRole.User);
                }

                var newAdmin = new User()
                {
                    UserName = "admin.admin",
                    Email = "admin.friends@gmail.com",
                    FullName = "Muthuraman Admin",
                    ProfilePictureUrl = "~/images/profilePictures/Muthuraman.jpg",
                    EmailConfirmed = true
                };

                var resultAdmin = await userManager.CreateAsync(newAdmin, userPassword);

                if (resultAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, AppRole.Admin);
                }
            }
        }
    }
}
