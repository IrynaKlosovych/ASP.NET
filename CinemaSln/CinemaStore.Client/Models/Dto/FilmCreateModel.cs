using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Models.Dto
{
    public class FilmCreateModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть назву фільму")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Опис")]
        [Required(ErrorMessage = "Вкажіть опис фільму")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Рейтинг 0–10")]
        [Required(ErrorMessage = "Вкажіть рейтинг")]
        [Range(0, 10, ErrorMessage = "Рейтинг повинен бути від 0 до 10")]
        public decimal Rating { get; set; }

        [Display(Name = "Ціна квитка")]
        [Required(ErrorMessage = "Вкажіть ціну квитка")]
        [Range(0, 1000, ErrorMessage = "Ціна повинна бути від 0 до 1000")]
        public decimal TicketPrice { get; set; }

        [Display(Name = "Жанр")]
        [Required(ErrorMessage = "Оберіть жанр")]
        public string Genre { get; set; } = string.Empty;

        [Display(Name = "Тривалість (хв)")]
        [Required(ErrorMessage = "Вкажіть тривалість")]
        [Range(1, 500, ErrorMessage = "Тривалість має бути більше 0 хв")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Дата виходу")]
        [Required(ErrorMessage = "Вкажіть дату виходу")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
    }
}
