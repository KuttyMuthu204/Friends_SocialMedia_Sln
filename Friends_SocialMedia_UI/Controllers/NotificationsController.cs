using Friends_Data.Data.Models;
using Friends_Data.Helpers.Concerns;
using Friends_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    [Authorize(Roles = AppRole.User)]
    public class NotificationsController : BaseController
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var count = await _notificationsService.GetUnReadNotificationCount(userId.Value);
            return Json(count);
        }

        public async Task<IActionResult> GetNotifications()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var notifications = await _notificationsService.GetNotification(userId.Value);
            return PartialView("Notifications/_Notifications", notifications);
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationAsRead(int notificationId)
        {
            await _notificationsService.SendNotificationReadAsync(notificationId);

            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var notifications = await _notificationsService.GetNotification(userId.Value);
            return PartialView("Notifications/_Notifications", notifications);
        }
    }
}
