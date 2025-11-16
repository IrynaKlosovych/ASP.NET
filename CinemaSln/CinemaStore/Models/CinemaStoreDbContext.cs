using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace CinemaStore.Models
{
    public class CinemaStoreDbContext: DbContext
    {
        public CinemaStoreDbContext(DbContextOptions<CinemaStoreDbContext> options) : base(options) { }
        public DbSet<Film> Films => Set<Film>();
        public DbSet<Screening> Screenings => Set<Screening>();
        public DbSet<Seat> Seats => Set<Seat>();
    }
}
