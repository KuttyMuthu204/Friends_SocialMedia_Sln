using Friends_App_Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IPostService _postService;

        public FavoritesController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> GetAllFavoritePosts()
        {
            int loggedInUserId = 1;

            var myFavPosts = await _postService.GetAllFavoritePostsAsync(loggedInUserId);
            return View("Index", myFavPosts);
        }
    }
}
