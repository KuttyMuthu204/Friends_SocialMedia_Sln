using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers.Base;

public abstract class BaseController : Controller
{
    protected int? GetUserId()
    {
        var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(loggedInUserId))
        {
            return null;
        }

        return int.Parse(loggedInUserId);
    }

    protected string GetUserFullName()
    {
        var loggedInFullName = User.FindFirstValue(ClaimTypes.Name);
        var givenName = User.FindFirstValue(ClaimTypes.GivenName);
        var surName = User.FindFirstValue(ClaimTypes.Surname);

        return loggedInFullName;
    }

    protected IActionResult RedirectToLogin()
    {
        return RedirectToAction("Login", "Authentication");
    }
}
