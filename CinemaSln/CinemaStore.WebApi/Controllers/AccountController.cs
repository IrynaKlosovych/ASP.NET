using CinemaStore.WebApi.Models;
using CinemaStore.WebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        public AccountController(UserManager<IdentityUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
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
            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Користувач з таким email вже існує" });
            }

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, role);

            return result;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await CreateUserAsync(model, "User");
            if (!result.Succeeded)
            {
                AddIdentityErrors(result);
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new { message = "Користувач зареєстрований успішно", token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new { message = "Користувача з такою адресою не існує" });

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordCorrect)
                return BadRequest(new { message = "Пароль неправильний" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new { message = "Успішний вхід", token });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new { message = "Користувач не знайдений" });

            return Ok(new ProfileModel
            {
                Email = user.Email,
                UserName = user.UserName
            });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new { message = "Користувач не знайдений" });

            bool updated = false;

            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    AddIdentityErrors(setEmailResult);
                    return BadRequest(ModelState);
                }
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName != user.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    AddIdentityErrors(setUserNameResult);
                    return BadRequest(ModelState);
                }
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    return BadRequest(new { message = "Поточний пароль обов'язковий для зміни пароля" });
                }

                var passResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    AddIdentityErrors(passResult);
                    return BadRequest(ModelState);
                }
                updated = true;
            }

            return Ok(new { message = updated ? "Дані оновлено!" : "Немає змін" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await CreateUserAsync(model, "Admin");
            if (!result.Succeeded)
            {
                AddIdentityErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new { message = "Адміністратор створений" });
        }
    }
}
