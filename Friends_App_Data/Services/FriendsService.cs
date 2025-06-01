using Friends_Data.Data;
using Friends_Data.Data.Models;
using Friends_Data.Data.Models;
using Friends_Data.Dtos;
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
            var friendShip = await _context.FriendShips.FirstOrDefaultAsync(n => n.Id == friendshipId);

            if (friendShip != null)
            {
                _context.FriendShips.Remove(friendShip);
                await _context.SaveChangesAsync();

                //friend requests
                var requests = await _context.FriendRequests
                    .Where(r => (r.SenderId == friendShip.SenderId && r.ReceiverId == friendShip.ReceiverId)
                    || (r.SenderId == friendShip.ReceiverId && r.ReceiverId == friendShip.SenderId)).ToListAsync();

                if (requests.Any())
                {
                    _context.FriendRequests.RemoveRange(requests);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<UserWithFriendsCountDto>> GetSuggestedFriendsAsync(int userId)
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
                .Select(n => new UserWithFriendsCountDto()
                {
                    User = n,
                    FriendsCount = _context.FriendShips.Count(f => f.SenderId == n.Id || f.ReceiverId == n.Id)
                }).Take(5).ToListAsync();

            return suggestedFriends;
        }

        public async Task<List<FriendRequest>> GetSentFriendRequestAsync(int userId)
        {
            var friendRequestsSent = await _context.FriendRequests
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Where(f => f.SenderId == userId && f.Status == FriendShipStatus.Pending)
                .ToListAsync();

            return friendRequestsSent;
        }

        public async Task<List<FriendRequest>> GetReceivedFriendRequestAsync(int userId)
        {
            var friendRequestsReceived = await _context.FriendRequests
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Where(f => f.ReceiverId == userId && f.Status == FriendShipStatus.Pending)
                .ToListAsync();

            return friendRequestsReceived;
        }

        public async Task<List<FriendShip>> GetFriendsAsync(int userId)
        {
            var friends = await _context.FriendShips
                         .Include(n => n.Sender).Include(n => n.Receiver).Where(n => n.SenderId == userId || n.ReceiverId == userId).ToListAsync();

            return friends;
        }
    }
}
