using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data.Models;

namespace Friends_Data.Services
{
    public interface IFriendsService
    {
        Task SendRequestAsync(int senderId, int receiverId);
        Task UpdateRequestAsync(int requestId, string status);
        Task RemoveFriendAsync(int friendshipId);
        Task<List<User>> GetSuggestedFriendsAsync(int userId);    
    }
}
