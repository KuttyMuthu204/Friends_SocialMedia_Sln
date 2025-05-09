using System.Security.Claims;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IPostService _postService;

        public FavoritesController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> GetAllFavoritePosts()
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var myFavPosts = await _postService.GetAllFavoritePostsAsync(loggedInUserId.Value);
            return View("Index", myFavPosts);
        }
    }
}
