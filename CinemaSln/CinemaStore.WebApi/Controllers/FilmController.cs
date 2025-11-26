using CinemaStore.Data.Models;
using CinemaStore.Data.Repositories;
using CinemaStore.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly ICinemaStoreRepository _context;

        public FilmController(ICinemaStoreRepository context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var films = _context.Films
                .Select(f => new FilmDto
                {
                    FilmID = f.FilmID,
                    Title = f.Title,
                    Genre = f.Genre,
                    Description = f.Description,
                    Rating = f.Rating,
                    TicketPrice = f.TicketPrice,
                    DurationMinutes = f.DurationMinutes,
                    ReleaseDate = f.ReleaseDate
                })
                .ToList();

            return Ok(films);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var film = _context.Films
                .Include(f => f.Screenings)
                .FirstOrDefault(f => f.FilmID == id);

            if (film == null) return NotFound();

            var dto = new FilmDto
            {
                FilmID = film.FilmID,
                Title = film.Title,
                Genre = film.Genre,
                Description = film.Description,
                Rating = film.Rating,
                TicketPrice = film.TicketPrice,
                DurationMinutes = film.DurationMinutes,
                ReleaseDate = film.ReleaseDate,
                Screenings = film.Screenings.Select(s => new ScreeningDto
                {
                    Id = s.Id,
                    Hall = s.Hall,
                    StartTime = s.StartTime,
                    IsOver = s.IsOver
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create([FromBody] Film model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Rating < 0 || model.Rating > 10) return BadRequest("Рейтинг має бути числом від 0 до 10");
            if (model.TicketPrice < 0) return BadRequest("Ціна повинна бути додатнім числом");
            if (model.DurationMinutes <= 0) return BadRequest("Тривалість має бути додатнім числом");

            _context.CreateFilm(model);
            return CreatedAtAction(nameof(GetById), new { id = model.FilmID }, model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, [FromBody] Film model)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmID == id);
            if (film == null) return NotFound();

            film.Title = model.Title;
            film.Description = model.Description;
            film.Genre = model.Genre;
            film.Rating = model.Rating;
            film.TicketPrice = model.TicketPrice;
            film.DurationMinutes = model.DurationMinutes;
            film.ReleaseDate = model.ReleaseDate;

            _context.UpdateFilm(film);
            return Ok(film);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmID == id);
            if (film == null) return NotFound();

            _context.DeleteFilm(film);
            return Ok(new { message = "Фільм видалено" });
        }
    }
}
