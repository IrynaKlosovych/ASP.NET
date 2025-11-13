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
    }
}

