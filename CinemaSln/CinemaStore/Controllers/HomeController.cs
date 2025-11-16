using CinemaStore.Models;
using CinemaStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(string genre, int page = 1)
        {
            var filmsQuery = repository.Films
        .Where(f => genre == null || f.Genre == genre);

            var films = filmsQuery
                .OrderBy(f => f.FilmID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            return View(new FilmsListViewModel
            {
                Films = films,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = filmsQuery.Count()
                },
                CurrentGenre = genre
            });
        }
    }
}