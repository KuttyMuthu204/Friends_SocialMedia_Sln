using Friends_Data.Helpers.Concerns;
using Friends_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    [Authorize(Roles = AppRole.Admin)]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IPostService _postService;

        public AdminController(IAdminService adminService, IPostService postService )
        {
            _adminService = adminService;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportedPosts()
        {
            var reportedPosts = await _adminService.GetReportedPostsAsync();   
            return View("Index", reportedPosts);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveReportedPost(int postId)
        {
            await _postService.ApproveReportedPost(postId);
            return RedirectToAction("GetReportedPosts");
        }

        [HttpPost]
        public async Task<IActionResult> CancelReportedPost(int postId)
        {
            await _postService.CancelReportedPost(postId);
            return RedirectToAction("GetReportedPosts");
        }
    }
}
