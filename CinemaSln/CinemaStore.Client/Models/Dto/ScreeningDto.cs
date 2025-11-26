using System.Diagnostics.Contracts;

namespace CinemaStore.Client.Models.Dto
{
    public class ScreeningDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public string Hall { get; set; }
    }
}
