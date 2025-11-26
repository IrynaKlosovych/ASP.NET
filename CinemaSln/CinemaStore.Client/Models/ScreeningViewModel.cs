using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Models
{
    public class ScreeningViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Фільм")]
        [Required(ErrorMessage = "Будь ласка, оберіть фільм")]
        public long FilmId { get; set; }

        [Display(Name = "Дата та час сеансу")]
        [Required(ErrorMessage = "Вкажіть дату та час сеансу")]
        public string StartTime { get; set; }

        [Display(Name = "Зал")]
        [Required(ErrorMessage = "Вкажіть номер залу")]
        [StringLength(50, ErrorMessage = "Назва залу не може перевищувати 50 символів")]
        public string Hall { get; set; }

        [Display(Name = "Сеанс завершено")]
        public bool IsOver { get; set; }

        //public IEnumerable<SelectListItem> Films { get; set; } = new List<SelectListItem>();
    }
}
