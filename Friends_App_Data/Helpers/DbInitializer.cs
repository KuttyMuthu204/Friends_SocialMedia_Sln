using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;

namespace Friends_App_Data.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext appContext)
        {
            if (!appContext.Users.Any() && !appContext.Posts.Any())
            {
                var newUser = new User()
                {
                    FullName = "Muthuraman S",
                    ProfilePictureUrl = "~/images/profilePictures/Muthuraman.jpg"
                };

                await appContext.Users.AddAsync(newUser);
                await appContext.SaveChangesAsync();

                var newPostWithUser = new Post()
                {
                    Content = "This is going to be our first post which is being loaded from the database." +
                    "and it has been created using our text user.",
                    ImageUrl = "",
                    NoOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UserId = newUser.Id,
                };

                var newPostWithImage = new Post()
                {
                    Content = "This is going to be our second post which is being loaded from the database." +
                    "and it has been created using our text user.",
                    ImageUrl = "https://th.bing.com/th/id/OIP.N8F8QgLuxGEUhZeovsyjJwHaFN?rs=1&pid=ImgDetMain",
                    NoOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UserId = newUser.Id,
                };

                await appContext.Posts.AddRangeAsync(newPostWithImage, newPostWithUser);
                await appContext.SaveChangesAsync();
            }
        }
    }
}
