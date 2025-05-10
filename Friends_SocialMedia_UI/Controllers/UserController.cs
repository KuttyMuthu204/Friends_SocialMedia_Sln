using Friends_App_Data.Data.Models;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Friends_UI.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly UserManager<User> _userManager;

        public UserController(IUsersService usersService, UserManager<User> userManager)
        {
            _usersService = usersService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId)
        {
            var allPosts = await _usersService.GetUserPosts(userId);
            var user = await _userManager.FindByIdAsync(userId.ToString());

            var userProfileVM = new GetUserProfileVM()
            {
                User = user,
                Posts = allPosts
            };

            return View(userProfileVM);
        }
    }
}
