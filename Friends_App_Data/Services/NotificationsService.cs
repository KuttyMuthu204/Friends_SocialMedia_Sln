using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data;
using Friends_Data.Data.Models;

namespace Friends_Data.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly AppDbContext _context;

        public NotificationsService(AppDbContext context)
        {
            _context = context;
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
        }
    }
}
