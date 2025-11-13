using CinemaStore.Models;
using CinemaStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;


namespace CinemaStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICinemaStoreRepository repository;
        public int PageSize = 3;

        public HomeController(ICinemaStoreRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(int page = 1)
        {
            var filmsListViewModel = new FilmsListViewModel
            {
                Films = repository.Films
                    .OrderBy(f => f.FilmID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Films.Count()
                }
            };

            return View(filmsListViewModel);
        }
    }
}