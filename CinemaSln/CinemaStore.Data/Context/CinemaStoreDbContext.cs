using System.Collections.Generic;
using CinemaStore.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace CinemaStore.Data.Context
{
    public class CinemaStoreDbContext: DbContext
    {
        public CinemaStoreDbContext(DbContextOptions<CinemaStoreDbContext> options) : base(options) { }
        public DbSet<Film> Films => Set<Film>();
        public DbSet<Screening> Screenings => Set<Screening>();
        public DbSet<Seat> Seats => Set<Seat>();
    }
}
