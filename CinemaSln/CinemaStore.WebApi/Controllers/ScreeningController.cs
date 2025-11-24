using CinemaStore.Data.Models;
using CinemaStore.Data.Repositories;
using CinemaStore.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        private readonly ICinemaStoreRepository _context;

        public ScreeningController(ICinemaStoreRepository context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var screenings = _context.Screenings
                .Include(s => s.Seats)
                .Select(s => new ScreeningDetailDto
                {
                    Id = s.Id,
                    FilmId = s.FilmId,
                    Hall = s.Hall,
                    StartTime = s.StartTime,
                    IsOver = s.IsOver,
                    Seats = s.Seats.Select(seat => new SeatDto
                    {
                        Id = seat.Id,
                        Row = seat.Row,
                        Number = seat.Number,
                        IsBooked = seat.IsBooked
                    }).ToList()
                }).ToList();

            return Ok(screenings);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var screening = _context.Screenings
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == id);

            if (screening == null) return NotFound();

            var dto = new ScreeningDetailDto
            {
                Id = screening.Id,
                FilmId = screening.FilmId,
                Hall = screening.Hall,
                StartTime = screening.StartTime,
                IsOver = screening.IsOver,
                Seats = screening.Seats.Select(seat => new SeatDto
                {
                    Id = seat.Id,
                    Row = seat.Row,
                    Number = seat.Number,
                    IsBooked = seat.IsBooked
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet("byfilm/{filmId}")]
        public IActionResult GetByFilm(long filmId)
        {
            var screenings = _context.Screenings
                .Include(s => s.Seats)
                .Where(s => s.FilmId == filmId)
                .OrderBy(s => s.StartTime)
                .Select(s => new ScreeningDetailDto
                {
                    Id = s.Id,
                    FilmId = s.FilmId,
                    Hall = s.Hall,
                    StartTime = s.StartTime,
                    IsOver = s.IsOver,
                    Seats = s.Seats.Select(seat => new SeatDto
                    {
                        Id = seat.Id,
                        Row = seat.Row,
                        Number = seat.Number,
                        IsBooked = seat.IsBooked
                    }).ToList()
                }).ToList();

            return Ok(screenings);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Screening model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.CreateScreening(model);
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Screening model)
        {
            var screening = _context.Screenings.FirstOrDefault(s => s.Id == id);
            if (screening == null) return NotFound();

            screening.FilmId = model.FilmId;
            screening.Hall = model.Hall;
            screening.IsOver = model.IsOver;
            screening.StartTime = model.StartTime;

            _context.UpdateScreening(screening);
            return Ok(screening);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var screening = _context.Screenings.FirstOrDefault(s => s.Id == id);
            if (screening == null) return NotFound();

            _context.DeleteScreening(screening);
            return Ok(new { message = "Сеанс видалено" });
        }

        [HttpPost("addseat")]
        public IActionResult AddSeat([FromBody] SeatRequest model)
        {
            var screening = _context.Screenings
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == model.ScreeningId);

            if (screening == null) return NotFound();

            if (!screening.Seats.Any(s => s.Row == model.Row && s.Number == model.Number))
            {
                screening.Seats.Add(new Seat
                {
                    ScreeningId = model.ScreeningId,
                    Row = model.Row,
                    Number = model.Number,
                    IsBooked = false
                });

                _context.UpdateScreening(screening);
            }

            return Ok(screening.Seats.Select(s => new SeatDto
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                IsBooked = s.IsBooked
            }));
        }

        [HttpPost("bookseat")]
        public IActionResult BookSeat([FromBody] SeatRequest model)
        {
            var screening = _context.Screenings
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == model.ScreeningId);

            if (screening == null) return NotFound();

            var seat = screening.Seats.FirstOrDefault(s => s.Row == model.Row && s.Number == model.Number);
            if (seat == null) return NotFound("Місце не знайдено");

            if (seat.IsBooked) return BadRequest("Місце вже заброньоване");

            seat.IsBooked = true;
            _context.UpdateScreening(screening);

            return Ok(new SeatDto
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                IsBooked = seat.IsBooked
            });
        }

        [HttpPost("cancellseat")]
        public IActionResult CancelSeat([FromBody] SeatRequest model)
        {
            var screening = _context.Screenings
                .Include(s => s.Seats)
                .FirstOrDefault(s => s.Id == model.ScreeningId);

            if (screening == null) return NotFound();

            var seat = screening.Seats.FirstOrDefault(s => s.Row == model.Row && s.Number == model.Number);
            if (seat == null) return NotFound("Місце не знайдено");

            seat.IsBooked = false;
            _context.UpdateScreening(screening);

            return Ok(new SeatDto
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                IsBooked = seat.IsBooked
            });
        }
    }
}
