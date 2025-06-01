using Friends_Data.Helpers.Constants;
using Friends_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Friends_UI.ViewModels.Friends;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;
        private readonly INotificationsService _notificationService;

        public FriendsController(IFriendsService friendsService, INotificationsService notificationService)
        {
            _friendsService = friendsService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var friendsData = new FriendShipVM()
            {
                Friends = await _friendsService.GetFriendsAsync(userId.Value),
                FriendRequestsSent = await _friendsService.GetSentFriendRequestAsync(userId.Value),
                FriendRequestsReceived = await _friendsService.GetReceivedFriendRequestAsync(userId.Value)
            };

            return View(friendsData);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(int receiverId)
        {
            var userId = GetUserId();
            var userFullName = GetUserFullName();

            if (!userId.HasValue)  RedirectToLogin();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);

            await _notificationService.AddNewNotificationAsync(receiverId, NotificationTypes.FriendRequest, userFullName, null);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {
            var userId = GetUserId();
            var userFullName = GetUserFullName();

            if (!userId.HasValue) RedirectToLogin();

            var request = await _friendsService.UpdateRequestAsync(requestId, status);

            if (status == FriendShipStatus.Accepted)
            {
                await _notificationService.AddNewNotificationAsync(request.SenderId, NotificationTypes.FriendRequestAccepted, userFullName, null);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriendRequest(int friendShipId)
        {
            await _friendsService.RemoveFriendAsync(friendShipId);
            return RedirectToAction("Index");
        }
    }
}
