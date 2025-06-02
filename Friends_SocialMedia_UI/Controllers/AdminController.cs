using Friends_Data.Services;
using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            var reportedPosts = _adminService.GetReportedPostsAsync();   
            return View(reportedPosts);
        }
    }
}
