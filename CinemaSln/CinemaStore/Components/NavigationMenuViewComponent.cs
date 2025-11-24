using Microsoft.AspNetCore.Mvc;
using CinemaStore.Models;
using CinemaStore.Models.ViewModels;
using CinemaStore.Data.Repositories;


namespace CinemaStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ICinemaStoreRepository repository;

        public NavigationMenuViewComponent(ICinemaStoreRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            var genre = repository.Films
                .Select(f => f.Genre)
                .Distinct()
                .OrderBy(c => c);

            string selectedGenre = RouteData?.Values["genre"] as string
                       ?? HttpContext.Request.Query["genre"];

            return View(new NavigationMenuViewModel
            {
                Genre = genre,
                SelectedGenre = selectedGenre
            });
        }
    }
}

