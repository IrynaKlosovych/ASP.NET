using System.ComponentModel.DataAnnotations;

namespace CinemaStore.WebApi.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [EmailAddress(ErrorMessage = "Некоректна електронна адреса")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [MinLength(8, ErrorMessage = "Пароль має містити мінімум 8 символів")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; }
    }
}
