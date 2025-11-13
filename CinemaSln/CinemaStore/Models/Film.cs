using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaStore.Models
{
    public class Film
    {
        public long? FilmID { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        [Column(TypeName = "decimal(3, 1)")]
        public decimal Rating { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal TicketPrice { get; set; }

        public string Genre { get; set; } = String.Empty;
        public int DurationMinutes { get; set; } 
        public DateTime ReleaseDate { get; set; }
    }
}
