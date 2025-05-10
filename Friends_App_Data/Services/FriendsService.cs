using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data;
using Friends_Data.Data.Models;
using Friends_Data.Helpers.Constants;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Friends_Data.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly AppDbContext _context;

        public FriendsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendRequest(int senderId, int receiverId)
        {
            var request = new FriendRequest()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendShipStatus.Pending,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _context.FriendRequests.AddAsync(request);    
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
