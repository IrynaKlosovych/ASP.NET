namespace CinemaStore.Models.ViewModels
{
    public class NavigationMenuViewModel
    {
        public IEnumerable<string> Genre { get; set; }
        public string SelectedGenre { get; set; }
    }
}
