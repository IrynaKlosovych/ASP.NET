using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Models
{
    public class ProfileModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Поточний пароль")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Підтвердження пароля")]
        public string? ConfirmPassword { get; set; }
    }
}
