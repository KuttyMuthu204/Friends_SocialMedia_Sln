using System.Security.Claims;
using Friends_App_Data.Data.Models;
using Friends_App_Data.Helpers.Enums;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;
        private readonly UserManager<User> _userManager;

        public SettingsController(IUsersService usersService, IFilesService filesService, UserManager<User> userManager)
        {
            _usersService = usersService;
            _filesService = filesService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _usersService.GetUser(int.Parse(loggedInUserId));
            var loggedInUser = await _userManager.GetUserAsync(User);
            return View(loggedInUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ProfilePictureVM profilePictureVM)
        {
            var loggedInUserId = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, ImageFileType.ProfilePicture);
            await _usersService.UpdateProfilePicture(loggedInUserId, uploadedProfilePictureUrl);
            return RedirectToAction("Index");
        }
    }
}
