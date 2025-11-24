using CinemaStore.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models.ViewModels
{
    public class FilmViewModel
    {
        public Film Film { get; set; } = new();

        public IEnumerable<SelectListItem> Genres { get; set; }
    }
}
