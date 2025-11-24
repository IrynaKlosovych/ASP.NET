namespace CinemaStore.WebApi.Models
{
    public class SeatRequest
    {
        public int ScreeningId { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
    }
}
