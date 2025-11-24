using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CinemaStore.Data.Models
{
    public class Screening
    {
        public int Id { get; set; }

        [Display(Name = "Фільм")]
        [Required(ErrorMessage = "Будь ласка, оберіть фільм")]
        public long FilmId { get; set; }

        [JsonIgnore]
        public Film? Film { get; set; }

        [Display(Name = "Дата та час сеансу")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Вкажіть дату та час сеансу")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Зал")]
        [Required(ErrorMessage = "Вкажіть номер залу")]
        [StringLength(50, ErrorMessage = "Назва залу не може перевищувати 50 символів")]
        public string Hall { get; set; }

        public List<Seat> Seats { get; set; } = new List<Seat>();

        [Display(Name = "Сеанс завершено")]
        public bool IsOver { get; set; }
    }
}
