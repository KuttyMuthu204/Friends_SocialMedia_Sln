using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_Data.Data.Models;

namespace Friends_Data.Services
{
    public interface INotificationsService
    {
        Task AddNewNotificationAsync(int userId, string notificationType, string userFullName, int? postId);
        Task<int> GetUnReadNotificationCount(int userId);
        Task<List<Notification>> GetNotification(int userId);
    }
}
