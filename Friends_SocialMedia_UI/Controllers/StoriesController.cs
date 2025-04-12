using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Friends_App_Data.Data;
using Friends_SocialMedia_UI.ViewModels.Stories;
using Friends_App_Data.Data.Models;

namespace Friends_SocialMedia_UI.Controllers
{
    public class StoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StoriesController> _logger;

        public StoriesController(AppDbContext context, ILogger<StoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            int loggedInUserId = 1;

            var newStory = new Story()
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId
            };

            if (storyVM.Image != null && storyVM.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                if (storyVM.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/stories");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(storyVM.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await storyVM.Image.CopyToAsync(stream);
                    }

                    //Set the URL to the newPost object
                    newStory.ImageUrl = "/images/stories/" + fileName;
                }
            }

            //Save the new story to the database
            await _context.Stories.AddAsync(newStory);
            await _context.SaveChangesAsync();

            return View("Home/Index");
        }
    }
}
