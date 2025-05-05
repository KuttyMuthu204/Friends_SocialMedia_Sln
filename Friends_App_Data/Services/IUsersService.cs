using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data.Models;

namespace Friends_App_Data.Services
{
    public interface IUsersService
    {
        Task<User> GetUser(int loggedInUserId);

        Task UpdateProfilePicture(int loggedInUserId, string profilePicUrl);
    }
}
