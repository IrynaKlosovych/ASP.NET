using System.ComponentModel.DataAnnotations;

namespace CinemaStore.WebApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Електронна адреса обов'язкова")]
        [EmailAddress(ErrorMessage = "Некоректна електронна адреса")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        public string Password { get; set; }
    }
}
