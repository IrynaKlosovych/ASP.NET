namespace CinemaStore.WebApi.Models
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public bool IsBooked { get; set; }
    }
}
