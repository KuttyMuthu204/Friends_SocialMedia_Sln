using Friends_App_Data.Data.Models;

namespace Friends_App_Data.Services
{
    public interface IUsersService
    {
        Task<User> GetUser(int loggedInUserId);

        Task UpdateProfilePicture(int loggedInUserId, string profilePicUrl);

        Task<List<Post>> GetUserPosts(int userId);
    }
}
