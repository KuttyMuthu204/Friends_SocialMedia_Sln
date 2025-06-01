using Friends_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
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
    }
}
