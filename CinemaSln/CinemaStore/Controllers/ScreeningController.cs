using CinemaStore.Infrastructure;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CinemaStore.Controllers
{
    public class ScreeningController : Controller
    {
        private readonly CinemaStoreDbContext _context;

        public ScreeningController(CinemaStoreDbContext context)
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

    }
}
