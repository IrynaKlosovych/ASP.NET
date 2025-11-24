using System.ComponentModel.DataAnnotations;

namespace CinemaStore.WebApi.Models
{
    public class ProfileModel
    {
        [EmailAddress(ErrorMessage = "Некоректна електронна адреса")]
        public string Email { get; set; }

        public string UserName { get; set; }

        public string? CurrentPassword { get; set; }

        [MinLength(8, ErrorMessage = "Пароль має містити мінімум 8 символів")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
        public string? ConfirmPassword { get; set; }
    }
}
