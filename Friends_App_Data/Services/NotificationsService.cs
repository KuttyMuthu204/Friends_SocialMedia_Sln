using Friends_Data.Data;
using Friends_Data.Data.Models;
using Friends_Data.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Friends_Data.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task AddNewNotificationAsync(int userId, string notificationType, string userFullName, int? postId)
        {
            var notification = new Notification()
            {
                UserId = userId,
                Message = GetPostMessage(notificationType, userFullName),
                Type = notificationType,
                IsRead = false,
                PostId = postId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var notificationNumber = await GetUnReadNotificationCount(userId);
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notificationNumber);
        }

        private string GetPostMessage(string notificationType, string userFullName)
        {
            var message = notificationType switch
            {
                "Like" => $"{userFullName} liked your post.",
                "Comment" => $"{userFullName} commented on your post.",
                "Favorite" => $"{userFullName} favorited your post.",
                "Follow" => $"{userFullName} started following you.",
                "Mention" => $"{userFullName} mentioned you in a post.",
                "Share" => $"{userFullName} shared your post.",
                "Report" => $"{userFullName} reported your post.",
                "PostCreated" => $"{userFullName} created a new post.",
                "PostUpdated" => $"{userFullName} updated a post.",
                "PostDeleted" => $"{userFullName} deleted a post.",
                "FriendRequest" => $"{userFullName} sent you a friend request.",
                "FriendRequestAccepted" => $"{userFullName} accepted your friend request.",
                "FriendRequestRejected" => $"{userFullName} rejected your friend request.",
                _ => $"You have a new notification from {userFullName}."
            };

            return message;
        }

        public async Task<int> GetUnReadNotificationCount(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<List<Notification>> GetNotification(int userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.CreatedDate).ToListAsync();
        }

        public async Task SendNotificationReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification != null)
            {
                notification.IsRead = true;
                notification.UpdatedDate = DateTime.UtcNow;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
