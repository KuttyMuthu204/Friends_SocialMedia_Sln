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

        public static async Task SeedAsync(AppDbContext appContext)
        {
            //if (!appContext.Users.Any() && !appContext.Posts.Any())
            //{
            //    var newUser = new User()
            //    {
            //        FullName = "Muthuraman S",
            //        ProfilePictureUrl = "~/images/profilePictures/Muthuraman.jpg"
            //    };

            //    await appContext.Users.AddAsync(newUser);
            //    await appContext.SaveChangesAsync();

            //    var newPostWithUser = new Post()
            //    {
            //        Content = "This is going to be our first post which is being loaded from the database." +
            //        "and it has been created using our text user.",
            //        ImageUrl = "",
            //        NoOfReports = 0,
            //        DateCreated = DateTime.UtcNow,
            //        DateUpdated = DateTime.UtcNow,
            //        UserId = newUser.Id,
            //    };

            //    var newPostWithImage = new Post()
            //    {
            //        Content = "This is going to be our second post which is being loaded from the database." +
            //        "and it has been created using our text user.",
            //        ImageUrl = "https://th.bing.com/th/id/OIP.N8F8QgLuxGEUhZeovsyjJwHaFN?rs=1&pid=ImgDetMain",
            //        NoOfReports = 0,
            //        DateCreated = DateTime.UtcNow,
            //        DateUpdated = DateTime.UtcNow,
            //        UserId = newUser.Id,
            //    };

            //    await appContext.Posts.AddRangeAsync(newPostWithImage, newPostWithUser);
            //    await appContext.SaveChangesAsync();
            //}
        }
    }
}
