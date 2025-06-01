using Friends_App_Data.Data.Models;
using Friends_App_Data.Helpers.Enums;
using Friends_App_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Friends_SocialMedia_UI.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    [Authorize]
    public class HomeController : BaseController
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
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var allPosts = await _postService.GetAllPostsAsync(loggedInUserId.Value);
            return View(allPosts);
        }

        public async Task<IActionResult> GetPosById(int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            return View("Details", post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var imageUploadPath = await _fileService.UploadImageAsync(post.Image, ImageFileType.PostImage);

            var newPost = new Post()
            {
                Content = post.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ImageUrl = imageUploadPath,
                NoOfReports = 0,
                UserId = loggedInUserId.Value
            };

            await _postService.CreatePostAsync(newPost);
            await _hashtagService.AddPostHashTagsAsync(post.Content);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            await _postService.TogglePostLikeAsync(postLikeVM.PostId, loggedInUserId.Value);

            var post = await _postService.GetPostByIdAsync(postLikeVM.PostId);
            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            //Create a new comment
            var newComment = new Comment()
            {
                UserId = loggedInUserId.Value,
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
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            await _postService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggedInUserId.Value);

            var post = await _postService.GetPostByIdAsync(postFavoriteVM.PostId);
            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            await _postService.TogglePostVisibilityAsync(postVisibilityVM.PostId, loggedInUserId.Value);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            await _postService.ReportPostAsync(postReportVM.PostId, loggedInUserId.Value);
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
