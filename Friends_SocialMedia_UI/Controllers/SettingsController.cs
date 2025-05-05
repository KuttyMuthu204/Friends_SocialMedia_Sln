using Friends_App_Data.Helpers.Enums;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IFilesService _filesService;

        public SettingsController(IUsersService usersService, IFilesService filesService)
        {
            _usersService = usersService;
            _filesService = filesService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var user = await _usersService.GetUser(loggedInUserId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ProfilePictureVM profilePictureVM)
        {
            var loggedInUserId = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, ImageFileType.ProfilePicture);
            await _usersService.UpdateProfilePicture(loggedInUserId, uploadedProfilePictureUrl);
            return RedirectToAction("Index");
        }

        [HttpPost] 
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVM)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVM)
        {
            return RedirectToAction("Index");
        } 
    }
}
