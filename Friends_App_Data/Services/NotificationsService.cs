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

        public async Task AddNewNotificationAsync(int userId, string message, string notificationType)
        {
            var notification = new Notification()
            {
                UserId = userId,
                Message = message,
                Type = notificationType,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var notificationNumber = await GetUnReadNotificationCount(userId);
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notificationNumber);
        }

        public async Task<int> GetUnReadNotificationCount(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}
