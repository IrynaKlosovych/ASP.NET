namespace CinemaStore.Models
{
    public class Screening
    {
        public int Id { get; set; }
        public long FilmId { get; set; }
        public Film Film { get; set; }

        public DateTime StartTime { get; set; }
        public string Hall { get; set; }

        public List<Seat> Seats { get; set; } = new List<Seat>();
        public bool IsOver { get; set; }
    }
}
