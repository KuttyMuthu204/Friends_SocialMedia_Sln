using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Friends_App_Data.Helpers;
using Friends_App_Data.Helpers.Enums;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.ViewModels.Home;
using Friends_SocialMedia_UI.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_SocialMedia_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IHashtagService _hashtagService;
        private readonly IFilesService _fileService;

        public HomeController(IPostService postService, IHashtagService hashtagService, IFilesService filesService)
        {
            _postService = postService;
            _hashtagService = hashtagService;
            _fileService = filesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;

            var allPosts = await _postService.GetAllPostsAsync(loggedInUserId);
            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user
            int loggedInUser = 1;
            var imageUploadPath = await _fileService.UploadImageAsync(post.Image, ImageFileType.PostImage);

            var newPost = new Post()
            {
                Content = post.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = imageUploadPath,
                NoOfReports = 0,
                UserId = loggedInUser
            };

            await _postService.CreatePostAsync(newPost);
            await _hashtagService.AddPostHashTagsAsync(post.Content);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            int loggedInUser = 1;

            await _postService.TogglePostLikeAsync(postLikeVM.PostId, loggedInUser);
            return RedirectToAction("Index");
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

            await _postService.AddPostCommentAsync(newComment);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postService.RemovePostCommentAsync(removeCommentVM.CommentId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            int loggedInUser = 1;

            await _postService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggedInUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUser = 1;

            await _postService.TogglePostVisibilityAsync(postVisibilityVM.PostId, loggedInUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            int loggedInUser = 1;

            await _postService.ReportPostAsync(postReportVM.PostId, loggedInUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(PostDeleteVM postDeleteVM)
        {
            var postRemoved = await _postService.RemovePostAsync(postDeleteVM.PostId);
            await _hashtagService.RemovePostHashTagsAsync(postRemoved.Content);
            return RedirectToAction("Index");
        }
    }
}
