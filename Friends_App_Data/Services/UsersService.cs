using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Friends_App_Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _context.Users
                .Where(u => u.Id == loggedInUserId)
                .FirstOrDefaultAsync() ?? new User();
        }

        public async Task<List<Post>> GetUserPosts(int userId)
        {
            var posts = await _context.Posts
                               .Where(n => n.UserId == userId && n.Reports.Count < 5 && !n.IsDeleted)
                               .Include(u => u.User)
                               .Include(n => n.Likes)
                               .Include(f => f.Favorites)
                               .Include(n => n.Reports)
                               .Include(n => n.Comments)
                               .ThenInclude(n => n.User)
                               .OrderByDescending(o => o.DateCreated).ToListAsync();

            return posts;
        }

        public async Task UpdateProfilePicture(int loggedInUserId, string profilePicUrl)
        {
            var user = await _context.Users.Where(u => u.Id == loggedInUserId)
                                      .FirstOrDefaultAsync();
            if (user != null)
            {
                user.ProfilePictureUrl = profilePicUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
