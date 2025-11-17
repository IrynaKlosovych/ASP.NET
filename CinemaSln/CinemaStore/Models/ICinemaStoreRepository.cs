namespace CinemaStore.Models
{
    public interface ICinemaStoreRepository
    {
        IQueryable<Film> Films { get; }
        IQueryable<Screening> Screenings { get; }

        void CreateFilm(Film f);
        void UpdateFilm(Film f);
        void DeleteFilm(Film f);

        void CreateScreening(Screening s);
        void UpdateScreening(Screening s);
        void DeleteScreening(Screening s);
    }
}