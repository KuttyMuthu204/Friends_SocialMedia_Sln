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

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
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
            if (!userId.HasValue)  RedirectToLogin();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {
            await _friendsService.UpdateRequestAsync(requestId, status);
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
