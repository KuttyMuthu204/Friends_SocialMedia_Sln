using System.Security.Claims;
using Friends_Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.ViewComponent
{
    public class SuggestedFriendsViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly IFriendsService _friendsService;

        public SuggestedFriendsViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;   
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedInUserId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(int.Parse(loggedInUserId));
            return View(suggestedFriends);
        }
    }
}
