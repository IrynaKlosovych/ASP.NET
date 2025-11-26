using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної адреси")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; }
    }
}
