using Friends_App_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUsersService _usersService;

        public UserController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId)
        {
            var allPosts = await _usersService.GetUserPosts(userId);
            return View(allPosts);
        }
    }
}
