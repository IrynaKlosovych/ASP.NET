namespace CinemaStore.Data.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public bool IsBooked { get; set; }
        public Screening Screening { get; set; }
    }
}
