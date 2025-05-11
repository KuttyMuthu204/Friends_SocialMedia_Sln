using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Friends_Data.Data.Models;
using Friends_Data.Helpers.Constants;
using Microsoft.EntityFrameworkCore;

namespace Friends_Data.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly AppDbContext _context;

        public FriendsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateRequestAsync(int requestId, string status)
        {
            var request = await _context.FriendRequests.FindAsync(requestId);

            if (request != null)
            {
                request.Status = status;
                request.DateUpdated = DateTime.UtcNow;
                _context.FriendRequests.Update(request);
            }

            if (status == FriendShipStatus.Accepted)
            {
                var friendship = new FriendShip()
                {
                    SenderId = request.SenderId,
                    ReceiverId = request.ReceiverId,
                    DateCreated = DateTime.UtcNow,
                };

                await _context.FriendShips.AddAsync(friendship);
            }

            await _context.SaveChangesAsync();
        }

        public async Task SendRequestAsync(int requestId, int receiverId)
        {
            var request = new FriendRequest()
            {
                SenderId = requestId,
                ReceiverId = receiverId,
                Status = FriendShipStatus.Pending,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
            };
            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFriendAsync(int friendshipId)
        {
            var friendShip = await _context.FriendShips.FindAsync(friendshipId);

            if (friendShip != null)
            {
                _context.FriendShips.Remove(friendShip);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetSuggestedFriendsAsync(int userId)
        {
            var existingFriends = await _context.FriendShips.Where(f => f.SenderId == userId || f.ReceiverId == userId)
                .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
                .ToListAsync();

            //pending requests
            var pendingRequests = await _context.FriendRequests
                .Where(r => (r.SenderId == userId || r.ReceiverId == userId) && r.Status == FriendShipStatus.Pending)
                .Select(r => r.SenderId == userId ? r.ReceiverId : r.SenderId)
                .ToListAsync();

            //get suggested friends
            var suggestedFriends = await _context.Users
                .Where(n => n.Id != userId && !existingFriends.Contains(n.Id) && !pendingRequests.Contains(n.Id))
                .Take(5).ToListAsync();

            return suggestedFriends;
        }
    }
}
