using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [EmailAddress]
        [Display(Name = "Електронна адреса")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string ConfirmPassword { get; set; }
    }
}
