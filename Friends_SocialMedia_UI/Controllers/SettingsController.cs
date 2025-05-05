using Friends_App_Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;

        public SettingsController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var user = await _usersService.GetUser(loggedInUserId);
            return View(user);
        }
    }
}
