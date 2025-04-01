using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Friends_SocialMedia_UI.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_SocialMedia_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsyncs()
        {
            return View("Index", await _context.Posts.Include(u => u.User).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user
            int loggedInUser = 1;

            var newPost = new Post()
            {
                Content = post.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = "",
                NoOfReports = 0,
                UserId = loggedInUser
            };

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            //Redirect to the home page
            return RedirectToAction("GetAllPostsAsyncs");
        }
    }
}
