using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaStore.Models
{
    public class EFStoreRepository: ICinemaStoreRepository
    {
        private CinemaStoreDbContext context;
        public EFStoreRepository(CinemaStoreDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Film> Films => context.Films;
        public IQueryable<Screening> Screenings => context.Screenings.Include(s => s.Seats);

        public void CreateFilm(Film f)
        {
            context.Films.Add(f);
            context.SaveChanges();
        }

        public void CreateScreening(Screening s)
        {
            context.Screenings.Add(s);
            context.SaveChanges();
        }

        public void DeleteFilm(Film f)
        {
            context.Films.Remove(f);
            context.SaveChanges();
        }

        public void DeleteScreening(Screening s)
        {
            context.Screenings.Remove(s);
            context.SaveChanges();
        }

        public void UpdateFilm(Film f)
        {
            context.Films.Update(f);
            context.SaveChanges();
        }

        public void UpdateScreening(Screening s)
        {
            context.Screenings.Update(s);
            context.SaveChanges();
        }
    }
}

