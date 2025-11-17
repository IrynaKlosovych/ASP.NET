using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models.ViewModels
{
    public class FilmBindViewModel
    {
        public int FilmID { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть назву фільму")]
        public string Title { get; set; }

        [Display(Name = "Опис")]
        [Required(ErrorMessage = "Вкажіть опис фільму")]
        public string Description { get; set; }

        [Display(Name = "Рейтинг 0–10")]
        [Required(ErrorMessage = "Вкажіть рейтинг")]
        public string Rating { get; set; }

        [Display(Name = "Ціна квитка")]
        [Required(ErrorMessage = "Вкажіть ціну квитка")]
        public string TicketPrice { get; set; }

        [Display(Name = "Жанр")]
        [Required(ErrorMessage = "Оберіть жанр")]
        public string Genre { get; set; }

        [Display(Name = "Тривалість (хв)")]
        [Required(ErrorMessage = "Вкажіть тривалість")]
        public string DurationMinutes { get; set; }

        [Display(Name = "Дата виходу")]
        [Required(ErrorMessage = "Вкажіть дату виходу")]
        public string ReleaseDate { get; set; }

        [BindNever]
        [ValidateNever]
        public IEnumerable<SelectListItem> Genres { get; set; }
    }
}
