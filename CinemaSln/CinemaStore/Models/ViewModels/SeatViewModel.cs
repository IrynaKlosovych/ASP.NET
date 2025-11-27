namespace CinemaStore.Models.ViewModels
{
    public class SeatViewModel
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public bool IsBooked { get; set; }
        public string? BookedByUserId { get; set; }
    }
}
