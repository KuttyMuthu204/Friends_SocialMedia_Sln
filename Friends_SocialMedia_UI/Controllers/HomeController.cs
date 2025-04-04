using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Friends_App_Data.Migrations;
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
            return View("Index", await _context.Posts.Include(u => u.User).Include(n => n.Likes).Include(n => n.Comments).ThenInclude(n => n.User).OrderByDescending(o => o.DateCreated).ToListAsync());
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

            //Check and save the image
            if (post.Image != null && post.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                if (post.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/uploaded");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.Image.CopyToAsync(stream);
                    }

                    //Set the URL to the newPost object
                    newPost.ImageUrl = $"/images/uploaded/{fileName}";
                }
            }

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            //Redirect to the home page
            return RedirectToAction("GetAllPostsAsyncs");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            int loggedInUser = 1;

            //Chekc if the user has already liked the post
            var likes = await _context.Likes.Where(l => l.PostId == postLikeVM.PostId && l.UserId == loggedInUser).FirstOrDefaultAsync();

            if (likes != null)
            {
                _context.Likes.Remove(likes);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newLikes = new Like()
                {
                    PostId = postLikeVM.PostId,
                    UserId = loggedInUser
                };

                await _context.Likes.AddAsync(newLikes);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("GetAllPostsAsyncs");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            int loggedInUser = 1;

            //Create a new comment
            var newComment = new Comment()
            {
                UserId = loggedInUser,
                PostId = postCommentVM.PostId,
                Content = postCommentVM.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllPostsAsyncs");
        }
    }
}
