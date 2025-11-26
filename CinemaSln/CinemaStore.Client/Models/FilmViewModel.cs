using CinemaStore.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Client.Models
{
    public class FilmViewModel
    {
        public Film Film { get; set; } = new();
        public string Genre { get; set; }
    }
}
