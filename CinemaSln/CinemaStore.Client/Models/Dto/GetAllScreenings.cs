namespace CinemaStore.Client.Models.Dto
{
    public class GetAllScreenings
    {
        public int Id { get; set; }
        public long FilmId { get; set; }
        public string FilmTitle { get; set; }
        public string Hall { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsOver { get; set; }
        public List<SeatDto> Seats { get; set; } = new();
    }
}
