using System.Data;
using System.Security.Claims;
using Friends_Data.Data.Models;
using Friends_Data.Helpers.Concerns;
using Friends_Data.Helpers.Constants;
using Friends_SocialMedia_UI.ViewModels.Authentication;
using Friends_SocialMedia_UI.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var loggedInUser = await _userManager.FindByEmailAsync(loginVM.Email);

            if (loggedInUser == null)
            {
                ModelState.AddModelError("", "Invalid email or password. Please, try again");
                return View(loginVM);
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(loggedInUser);

            if (!existingUserClaims.Any(c => c.Type == CustomClass.FullName))
            {
                await _userManager.AddClaimAsync(loggedInUser, new Claim(CustomClass.FullName, loggedInUser.FullName));
            }

            var result = await _signInManager.PasswordSignInAsync(loggedInUser.UserName, loginVM.Password, false, false);

            if (result.Succeeded)
            {
                if (loginVM.Email == "admin.friends@gmail.com")
                {
                    await _userManager.AddToRoleAsync(loggedInUser, AppRole.Admin);
                    return RedirectToAction("GetReportedPosts", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password. Please, try again");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var newUser = new User()
            {
                FullName = registerVM.FirstName + " " + registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.Email
            };

            var existingUser = await _userManager.FindByEmailAsync(registerVM.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(registerVM);
            }

            var result = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, AppRole.User);
                await _userManager.AddClaimAsync(newUser, new Claim(CustomClass.FullName, newUser.FullName));
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVM)
        {
            if (updatePasswordVM.ConfirmPassword != updatePasswordVM.NewPassword)
            {
                TempData["PasswordError"] = "Passwords do not match";
                TempData["ActiveTab"] = "Password";
                return RedirectToAction("Index", "Settings");
            }

            var user = await _userManager.GetUserAsync(User);
            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, updatePasswordVM.CurrentPassword);

            if (!isCurrentPasswordValid)
            {
                TempData["PasswordError"] = "Current password is invalid";
                TempData["ActiveTab"] = "Password";
                return RedirectToAction("Index", "Settings");
            }

            var result = await _userManager.ChangePasswordAsync(user, updatePasswordVM.CurrentPassword, updatePasswordVM.NewPassword);

            if (result.Succeeded)
            {
                TempData["PasswordSuccess"] = "Password updated successfully";
                TempData["ActiveTab"] = "Password";
                await _signInManager.RefreshSignInAsync(user);
            }
            else
            {
                TempData["PasswordError"] = "Failed to update password";
                TempData["ActiveTab"] = "Password";
            }

            return RedirectToAction("Index", "Settings");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                user.FullName = profileVM.FullName;
                user.Bio = profileVM.Bio;
                user.UserName = profileVM.UserName;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    TempData["UserProfileError"] = "User profile could not be updated";
                    TempData["ActiveTab"] = "Profile";
                }
                else
                {
                    TempData["UserProfileSuccess"] = "User profile updated successfully";
                    TempData["ActiveTab"] = "Profile";
                    await _signInManager.RefreshSignInAsync(user);
                }
            }

            return RedirectToAction("Index", "Settings");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
