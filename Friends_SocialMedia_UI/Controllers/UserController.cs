using Friends_SocialMedia_UI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Friends_UI.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string userId)
        {
            return View();
        }
    }
}
