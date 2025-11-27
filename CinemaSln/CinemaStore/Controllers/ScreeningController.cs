using CinemaStore.Data.Models;
using CinemaStore.Data.Repositories;
using CinemaStore.Hubs;
using CinemaStore.Infrastructure;
using CinemaStore.Models;
using CinemaStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace CinemaStore.Controllers
{
    public class ScreeningController : Controller
    {
        private readonly ICinemaStoreRepository _context;
        private readonly IHubContext<SeatHub> _seatHub;


        public ScreeningController(ICinemaStoreRepository context, IHubContext<SeatHub> seatHub)
        {
            _context = context;
            _seatHub = seatHub;
        }

        [Authorize]
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

        [Authorize]
        public IActionResult Choose(int screeningId)
        {
            var screening = _context.Screenings
                .Include(s => s.Seats)
                .Include(s=>s.Film)
                .FirstOrDefault(s => s.Id == screeningId);

            if (screening == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var seats = screening.Seats.Select(s => new SeatViewModel
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                IsBooked = s.IsBooked,
                BookedByUserId = s.BookedByUserId
            }).ToList();

            ViewData["CurrentUserId"] = currentUserId;

            return View(new ScreeningWithSeatsViewModel
            {
                ScreeningId = screening.Id,
                FilmTitle = screening.Film?.Title,
                StartTime = screening.StartTime,
                Hall = screening.Hall,
                Seats = seats
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddSeat(int screeningId, string seat)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var screening = _context.Screenings.FirstOrDefault(s => s.Id == screeningId);
            if (screening == null) return NotFound();

            var s = screening.Seats.FirstOrDefault(x => (x.Row + x.Number) == seat);
            if (s == null) return NotFound();
            if (s.IsBooked) return BadRequest("Місце вже заброньоване");

            s.IsBooked = true;
            s.BookedByUserId = currentUserId;
            _context.UpdateScreening(screening);

            await _seatHub.Clients.All.SendAsync("SeatAdded", screeningId, seat, currentUserId);

            return Ok(new { seat, userId = currentUserId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveSeat(int screeningId, string seat)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var screening = _context.Screenings.FirstOrDefault(s => s.Id == screeningId);
            if (screening == null) return NotFound();

            var s = screening.Seats.FirstOrDefault(x => (x.Row + x.Number) == seat);
            if (s == null || s.BookedByUserId != currentUserId)
                return BadRequest("Неможливо зняти бронь");

            s.IsBooked = false;
            s.BookedByUserId = null;
            _context.UpdateScreening(screening); 

            await _seatHub.Clients.All.SendAsync("SeatRemoved", screeningId, seat, currentUserId);

            return Ok(new { seat, userId = currentUserId });
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSeats(int screeningId)
        {
            var selectedSeatsDict = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats")
                                    ?? new Dictionary<int, List<string>>();


            var seats = HttpContext.Session.GetJson<Dictionary<int, List<string>>>("SelectedSeats");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(seats));


            return Json(selectedSeatsDict.ContainsKey(screeningId) ? selectedSeatsDict[screeningId] : new List<string>());
        }

        [Authorize]
        public IActionResult Index()
        {
            var screenings = _context.Screenings
                .Include(s => s.Film)
                .ToList();

            return View(screenings);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var screening = _context.Screenings
                .Include(s => s.Film)
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == id);

            if (screening == null) return NotFound();

            return View(screening);
        }

        [Authorize]
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
        [Authorize]
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

        [Authorize]
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
        [Authorize]
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

        [Authorize]
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
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            var screening = _context.Screenings.FirstOrDefault(s => s.Id == id);
            if (screening == null) return NotFound();

            _context.DeleteScreening(screening);
            return RedirectToAction("Index");
        }

    }
}
