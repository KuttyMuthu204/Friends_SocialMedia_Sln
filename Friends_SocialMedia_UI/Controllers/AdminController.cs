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

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportedPosts()
        {
            var reportedPosts = await _adminService.GetReportedPostsAsync();   
            return View("Index", reportedPosts);
        }
    }
}
