namespace CinemaStore.Models.ViewModels
{
    public class ScreeningWithSeatsViewModel
    {
        public int ScreeningId { get; set; }
        public string FilmTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public string Hall { get; set; } = string.Empty;
        public List<SeatViewModel> Seats { get; set; } = new List<SeatViewModel>();
    }
}
