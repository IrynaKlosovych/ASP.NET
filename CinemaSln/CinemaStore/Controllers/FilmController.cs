using CinemaStore.Models;
using CinemaStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaStore.Controllers
{
    public class FilmController : Controller
    {
        private readonly ICinemaStoreRepository _context;

        public FilmController(ICinemaStoreRepository context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var films = _context.Films.ToList();
            return View(films);
        }

        public IActionResult Details(int id)
        {
            var film = _context.Films
                .Include(f => f.Screenings)
                .FirstOrDefault(f => f.FilmID == id);

            if (film == null) return NotFound();

            return View(film);
        }

        public IActionResult Create()
        {
            return View(new FilmBindViewModel
            {
                Genres = _context.Films
                    .Select(f => f.Genre)
                    .Distinct()
                    .OrderBy(g => g)
                    .Select(g => new SelectListItem { Value = g, Text = g })
            });
        }


        private IEnumerable<SelectListItem> GetGenres()
        {
            return _context.Films
                .Select(f => f.Genre)
                .Distinct()
                .OrderBy(g => g)
                .Select(g => new SelectListItem { Value = g, Text = g });
        }

        [HttpPost]
        public IActionResult Create(FilmBindViewModel model)
        {
            if (!decimal.TryParse(model.Rating, out decimal rating) || rating < 0 || rating > 10)
                ModelState.AddModelError("Rating", "Рейтинг має бути числом від 0 до 10");

            if (!decimal.TryParse(model.TicketPrice, out decimal price) || price < 0)
                ModelState.AddModelError("TicketPrice", "Ціна повинна бути додатнім числом");

            if (!int.TryParse(model.DurationMinutes, out int duration) || duration <= 0)
                ModelState.AddModelError("DurationMinutes", "Тривалість має бути додатним числом");

            if (!DateTime.TryParseExact(model.ReleaseDate, "dd.MM.yyyy", null,
                System.Globalization.DateTimeStyles.None, out DateTime releaseDate))
                ModelState.AddModelError("ReleaseDate", "Дата має бути у форматі дд.мм.рррр");

            if (!ModelState.IsValid)
            {
                model.Genres = GetGenres();
                return View(model);
            }

            var film = new Film
            {
                Title = model.Title,
                Description = model.Description,
                Rating = rating,
                TicketPrice = price,
                Genre = model.Genre,
                DurationMinutes = duration,
                ReleaseDate = releaseDate
            };

            _context.CreateFilm(film);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmID == id);
            if (film == null) return NotFound();

            var model = new FilmBindViewModel
            {
                Title = film.Title,
                Description = film.Description,
                Rating = film.Rating.ToString(),
                TicketPrice = film.TicketPrice.ToString(),
                Genre = film.Genre,
                DurationMinutes = film.DurationMinutes.ToString(),
                ReleaseDate = film.ReleaseDate.ToString("dd.MM.yyyy"),
                Genres = GetGenres()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, FilmBindViewModel model)
        {
            if (!decimal.TryParse(model.Rating, out decimal rating) || rating < 0 || rating > 10)
                ModelState.AddModelError("Rating", "Рейтинг має бути числом від 0 до 10");

            if (!decimal.TryParse(model.TicketPrice, out decimal price) || price < 0)
                ModelState.AddModelError("TicketPrice", "Ціна повинна бути додатнім числом");

            if (!int.TryParse(model.DurationMinutes, out int duration) || duration <= 0)
                ModelState.AddModelError("DurationMinutes", "Тривалість має бути додатнім числом");

            if (!DateTime.TryParseExact(model.ReleaseDate, "dd.MM.yyyy", null,
                System.Globalization.DateTimeStyles.None, out DateTime releaseDate))
                ModelState.AddModelError("ReleaseDate", "Дата має бути у форматі дд.мм.рррр");

            if (!ModelState.IsValid)
            {
                model.Genres = GetGenres();
                return View(model);
            }

            var film = _context.Films.FirstOrDefault(f => f.FilmID == id);
            if (film == null) return NotFound();

            film.Title = model.Title;
            film.Description = model.Description;
            film.Rating = rating;
            film.TicketPrice = price;
            film.Genre = model.Genre;
            film.DurationMinutes = duration;
            film.ReleaseDate = releaseDate;

            _context.UpdateFilm(film);

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmID == id);
            if (film == null) return NotFound();

            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmID == id);
            if (film == null) return NotFound();

            _context.DeleteFilm(film);
            return RedirectToAction("Index");
        }
    }
}
