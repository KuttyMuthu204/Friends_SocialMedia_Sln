using Friends_App_Data.Data.Models;
using Friends_App_Data.Helpers.Enums;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Friends_SocialMedia_UI.ViewModels.Stories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    [Authorize]
    public class StoriesController : BaseController
    {
        private readonly IStoriesService _storiesService;
        private readonly IFilesService _fileService;

        public StoriesController(IStoriesService storiesService, IFilesService fileService)
        {
            _storiesService = storiesService;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();
            var imageUploadPath = await _fileService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

            var newStory = new Story()
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                ImageUrl = imageUploadPath,
                UserId = loggedInUserId.Value
            };

            await _storiesService.CreateStoryAsync(newStory);
            return RedirectToAction("Index", "Home");
        }
    }
}
