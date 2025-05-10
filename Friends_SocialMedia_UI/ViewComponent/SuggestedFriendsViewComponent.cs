using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.ViewComponent
{
    public class SuggestedFriendsViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var suggestedFriends = new List<string>
            {
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown"
            };
            return View(suggestedFriends);
        }
    }
}
