using CinemaStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                string message = error.Description;

                if (message.Contains("Passwords must be at least"))
                    message = "Пароль має містити мінімум 8 символів";
                else if (message.Contains("Passwords must have at least one digit"))
                    message = "Пароль має містити хоча б одну цифру";
                else if (message.Contains("Passwords must have at least one uppercase"))
                    message = "Пароль має містити хоча б одну велику літеру";
                else if (message.Contains("Passwords must have at least one lowercase"))
                    message = "Пароль має містити хоча б одну малу літеру";

                ModelState.AddModelError("", message);
            }
        }

        private async Task<IdentityResult> CreateUserAsync(RegisterModel model, string role)
        {
            var existing = await userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("Email", "Користувач з таким email вже існує");
                return IdentityResult.Failed();
            }

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, role);

            return result;
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await CreateUserAsync(model, "User");

            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            AddIdentityErrors(result);

            return View(model);
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Користувача з такою адресою не існує");
                return View(model);
            }

            var passwordCorrect = await userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordCorrect)
            {
                ModelState.AddModelError("Password", "Пароль неправильний");
                return View(model);
            }

            await signInManager.SignInAsync(user, false);
            return Redirect(model.ReturnUrl ?? "/");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            return View(new ProfileModel
            {
                Email = user.Email,
                UserName = user.UserName
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            bool updated = false;

            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    AddIdentityErrors(setEmailResult);
                    return View(model);
                }
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName != user.UserName)
            {
                var setUserNameResult = await userManager.SetUserNameAsync(user, model.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    AddIdentityErrors(setUserNameResult);
                    return View(model);
                }
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Поточний пароль обов'язковий для зміни пароля");
                    return View(model);
                }

                var passResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    AddIdentityErrors(passResult);
                    return View(model);
                }
                updated = true;
            }

            if (updated)
                ViewBag.Message = "Дані оновлено!";

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await CreateUserAsync(model, "Admin");

            if (result.Succeeded)
            {
                TempData["AdminMessage"] = "Адміністратор створений!";
                return RedirectToAction("AddAdmin");
            }

            AddIdentityErrors(result);
            return View(model);
        }
    }
}
