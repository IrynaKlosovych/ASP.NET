namespace CinemaStore.WebApi.Models
{
    public class ScreeningDto
    {
        public int Id { get; set; }
        public string Hall { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsOver { get; set; }
    }
}
