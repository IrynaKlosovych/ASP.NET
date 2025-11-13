namespace CinemaStore.Models
{
    public interface ICinemaStoreRepository
    {
        IQueryable<Film> Films { get; }
    }
}