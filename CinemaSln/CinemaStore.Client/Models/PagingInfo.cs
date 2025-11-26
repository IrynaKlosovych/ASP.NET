namespace CinemaStore.Client.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }   
        public int ItemsPerPage { get; set; } = 3;  
        public int CurrentPage { get; set; } = 1;    

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;
    }
}
