using CinemaStore.Client.Services;
using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Models.Dto
{
    public class ScreeningCreateModel
    {
        [Required(ErrorMessage = "Виберіть фільм")]
        public long FilmId { get; set; }

        [Required(ErrorMessage = "Вкажіть дату та час")]
        [ValidateDateTime(ErrorMessage = "Невірний формат дати і часу")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "Вкажіть зал")]
        public string Hall { get; set; } = string.Empty;

        public bool IsOver { get; set; } = false;
    }
}
