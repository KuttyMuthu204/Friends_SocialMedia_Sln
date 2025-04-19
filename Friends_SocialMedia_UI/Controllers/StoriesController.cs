using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Friends_App_Data.Data;
using Friends_SocialMedia_UI.ViewModels.Stories;
using Friends_App_Data.Data.Models;
using Friends_App_Data.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Friends_App_Data.Services;

namespace Friends_SocialMedia_UI.Controllers
{
    public class StoriesController : Controller
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
            int loggedInUserId = 1;
            var imageUploadPath = await _fileService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

            var newStory = new Story()
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                ImageUrl = imageUploadPath,
                UserId = loggedInUserId
            }; 

            await _storiesService.CreateStoryAsync(newStory);
            return RedirectToAction("Index", "Home");
        }
    }
}
