using CinemaStore.Data.Models;

namespace CinemaStore.Models.ViewModels
{
    public class FilmsListViewModel
    {
        public IEnumerable<Film> Films { get; set; } = Enumerable.Empty<Film>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();

        public string? CurrentGenre { get; set; }
    }
}
