namespace CinemaStore.WebApi.Models
{
    public class ScreeningDetailDto
    {
        public int Id { get; set; }
        public long FilmId { get; set; }
        public string Hall { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsOver { get; set; }
        public List<SeatDto> Seats { get; set; } = new();
    }
}
