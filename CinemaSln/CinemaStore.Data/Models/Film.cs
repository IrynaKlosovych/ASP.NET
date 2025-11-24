using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaStore.Data.Models
{
    public class Film
    {
        public long? FilmID { get; set; }

        [Required(ErrorMessage = "Вкажіть назву фільму")]
        [StringLength(100, ErrorMessage = "Назва не повинна перевищувати 100 символів")]
        [Display(Name = "Назва")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть опис фільму")]
        [StringLength(1000, ErrorMessage = "Опис не повинен перевищувати 1000 символів")]
        [Display(Name = "Опис")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 10, ErrorMessage = "Рейтинг повинен бути від 0 до 10")]
        [Column(TypeName = "decimal(3,1)")]
        [Display(Name = "Рейтинг")]
        public decimal Rating { get; set; }

        [Range(0, 1000, ErrorMessage = "Ціна повинна бути від 0 до 1000")]
        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Ціна квитка")]
        public decimal TicketPrice { get; set; }

        [Required(ErrorMessage = "Вкажіть жанр")]
        [Display(Name = "Жанр")]
        public string Genre { get; set; } = string.Empty;

        [Range(1, 500, ErrorMessage = "Тривалість має бути більше 0 хв")]
        [Display(Name = "Тривалість (хв)")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Вкажіть дату виходу")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата виходу")]
        public DateTime ReleaseDate { get; set; }

        public List<Screening> Screenings { get; set; } = new();
    }
}
