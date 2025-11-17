using CinemaStore.Infrastructure;
using CinemaStore.Models;
using CinemaStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CinemaStore.Controllers
{
    public class ScreeningController : Controller
    {
        private readonly ICinemaStoreRepository _context;

        public ScreeningController(ICinemaStoreRepository context)
        {
            _context = context;
        }

        public IActionResult ByFilm(long filmId)
        {
            var screenings = _context.Screenings
                .Include(s => s.Film)
                .Where(s => s.FilmId == filmId)
                .OrderBy(s => s.StartTime)
                .ToList();

            ViewBag.FilmTitle = _context.Films
                .Where(f => f.FilmID == filmId)
                .Select(f => f.Title)
                .FirstOrDefault();

            return View(screenings);
        }

        public IActionResult Choose(int screeningId)
        {
            var screening = _context.Screenings
                .Include(s => s.Film)
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == screeningId);

            if (screening == null)
                return NotFound();

            return View(screening);
        }

        [HttpPost]
        public IActionResult AddSeat(int screeningId, string seat)
        {
            var selectedSeatsDict = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats")
                                    ?? new Dictionary<int, List<string>>();

            if (!selectedSeatsDict.ContainsKey(screeningId))
                selectedSeatsDict[screeningId] = new List<string>();

            if (!selectedSeatsDict[screeningId].Contains(seat))
                selectedSeatsDict[screeningId].Add(seat);

            HttpContext.Session.SetJson("SelectedSeats", selectedSeatsDict);


            var seats = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(seats));

            return Ok(selectedSeatsDict[screeningId]);
        }

        [HttpGet]
        public IActionResult GetSeats(int screeningId)
        {
            var selectedSeatsDict = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats")
                                    ?? new Dictionary<int, List<string>>();


            var seats = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(seats));


            return Json(selectedSeatsDict.ContainsKey(screeningId) ? selectedSeatsDict[screeningId] : new List<string>());
        }

        [HttpPost]
        public IActionResult RemoveSeat(int screeningId, string seat)
        {
            var selectedSeatsDict = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats");

            if (selectedSeatsDict != null && selectedSeatsDict.ContainsKey(screeningId))
            {
                selectedSeatsDict[screeningId].Remove(seat);

                if (selectedSeatsDict[screeningId].Count == 0)
                    selectedSeatsDict.Remove(screeningId);

                HttpContext.Session.SetJson("SelectedSeats", selectedSeatsDict);
            }

            var seats = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(seats));

            return Ok(selectedSeatsDict != null && selectedSeatsDict.ContainsKey(screeningId)
                      ? selectedSeatsDict[screeningId]
                      : new List<string>());
        }

        public IActionResult Index()
        {
            var screenings = _context.Screenings
                .Include(s => s.Film)
                .ToList();

            return View(screenings);
        }

        public IActionResult Details(int id)
        {
            var screening = _context.Screenings
                .Include(s => s.Film)
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == id);

            if (screening == null) return NotFound();

            return View(screening);
        }

        public IActionResult Create()
        {
            var model = new ScreeningViewModel
            {
                Films = _context.Films
                    .Select(f => new SelectListItem
                    {
                        Value = f.FilmID.ToString(),
                        Text = f.Title
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ScreeningViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Films = _context.Films
                    .Select(f => new SelectListItem { Value = f.FilmID.ToString(), Text = f.Title })
                    .ToList();

                return View(model);
            }

            if (!DateTime.TryParse(model.StartTime, out DateTime startTime))
            {
                ModelState.AddModelError("StartTime", "Невірний формат дати");
                model.Films = _context.Films
                    .Select(f => new SelectListItem { Value = f.FilmID.ToString(), Text = f.Title })
                    .ToList();
                return View(model);
            }

            var screening = new Screening
            {
                FilmId = model.FilmId,
                Hall = model.Hall,
                IsOver = model.IsOver,
                StartTime = startTime
            };

            _context.CreateScreening(screening);
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var screening = _context.Screenings.FirstOrDefault(s => s.Id == id);
            if (screening == null) return NotFound();

            var model = new ScreeningViewModel
            {
                Id = screening.Id,
                FilmId = screening.FilmId,
                Hall = screening.Hall,
                IsOver = screening.IsOver,
                StartTime = screening.StartTime.ToString("dd.MM.yyyy HH:mm"),
                Films = _context.Films
                    .Select(f => new SelectListItem
                    {
                        Value = f.FilmID.ToString(),
                        Text = f.Title
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ScreeningViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Films = _context.Films
                    .Select(f => new SelectListItem { Value = f.FilmID.ToString(), Text = f.Title })
                    .ToList();
                return View(model);
            }

            if (!DateTime.TryParseExact(model.StartTime, "dd.MM.yyyy HH:mm", null,
                System.Globalization.DateTimeStyles.None, out DateTime startTime))
            {
                ModelState.AddModelError("StartTime", "Невірний формат дати (дд.мм.рррр гг:хх)");
                model.Films = _context.Films
                    .Select(f => new SelectListItem { Value = f.FilmID.ToString(), Text = f.Title })
                    .ToList();
                return View(model);
            }

            var screening = _context.Screenings.FirstOrDefault(s => s.Id == model.Id);
            if (screening == null) return NotFound();

            screening.FilmId = model.FilmId;
            screening.Hall = model.Hall;
            screening.IsOver = model.IsOver;
            screening.StartTime = startTime;

            _context.UpdateScreening(screening);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var screening = _context.Screenings
                .FirstOrDefault(s => s.Id == id);

            if (screening == null) return NotFound();

            screening.Film = _context.Films
                .FirstOrDefault(f => f.FilmID == screening.FilmId);

            return View(screening);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var screening = _context.Screenings.FirstOrDefault(s => s.Id == id);
            if (screening == null) return NotFound();

            _context.DeleteScreening(screening);
            return RedirectToAction("Index");
        }

    }
}
