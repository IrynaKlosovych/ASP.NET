namespace CinemaStore.WebApi.Models
{
    public class FilmDto
    {
        public long? FilmID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Rating { get; set; }
        public decimal TicketPrice { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<ScreeningDto> Screenings { get; set; } = new();
    }
}
