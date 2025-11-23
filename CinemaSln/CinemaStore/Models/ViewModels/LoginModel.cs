using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [EmailAddress]
        [Display(Name = "Електронна адреса")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
