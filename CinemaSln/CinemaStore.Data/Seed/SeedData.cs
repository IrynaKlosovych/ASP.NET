using CinemaStore.Data.Context;
using CinemaStore.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace CinemaStore.Data.Seed
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            CinemaStoreDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<CinemaStoreDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Films.Any())
            {
                context.Films.AddRange(
                     new Film
                     {
                         Title = "Початок",
                         Description = "Талановитий злодій проникає у сни людей, щоб викрадати їхні секрети.",
                         Genre = "Наукова фантастика",
                         Rating = 8.8m,
                         TicketPrice = 250.00m,
                         DurationMinutes = 148,
                         ReleaseDate = new DateTime(2010, 7, 16)
                     },
                    new Film
                    {
                        Title = "Інтерстеллар",
                        Description = "Група дослідників вирушає крізь червоточину у пошуках нового дому для людства.",
                        Genre = "Пригоди",
                        Rating = 8.6m,
                        TicketPrice = 270.00m,
                        DurationMinutes = 169,
                        ReleaseDate = new DateTime(2014, 11, 7)
                    },
                    new Film
                    {
                        Title = "Темний лицар",
                        Description = "Бетмен бореться з новим ворогом — Джокером, який сіє хаос у Ґотемі.",
                        Genre = "Бойовик",
                        Rating = 9.0m,
                        TicketPrice = 230.00m,
                        DurationMinutes = 152,
                        ReleaseDate = new DateTime(2008, 7, 18)
                    },
                    new Film
                    {
                        Title = "Ла-Ла Ленд",
                        Description = "Музикант і актриса борються за свої мрії та кохання у Лос-Анджелесі.",
                        Genre = "Мюзикл",
                        Rating = 8.0m,
                        TicketPrice = 200.00m,
                        DurationMinutes = 128,
                        ReleaseDate = new DateTime(2016, 12, 9)
                    },
                    new Film
                    {
                        Title = "Паразити",
                        Description = "Бідна сім'я під виглядом працівників влаштовується до заможного дому.",
                        Genre = "Трилер",
                        Rating = 8.6m,
                        TicketPrice = 220.00m,
                        DurationMinutes = 132,
                        ReleaseDate = new DateTime(2019, 5, 30)
                    },
                    new Film
                    {
                        Title = "Аватар",
                        Description = "Людина опиняється у тілі іншої раси на планеті Пандора.",
                        Genre = "Фантастика",
                        Rating = 7.9m,
                        TicketPrice = 260.00m,
                        DurationMinutes = 162,
                        ReleaseDate = new DateTime(2009, 12, 18)
                    },
                    new Film
                    {
                        Title = "Форест Ґамп",
                        Description = "Історія простого чоловіка, який випадково стає свідком великих подій ХХ століття.",
                        Genre = "Драма",
                        Rating = 8.8m,
                        TicketPrice = 190.00m,
                        DurationMinutes = 142,
                        ReleaseDate = new DateTime(1994, 7, 6)
                    },
                    new Film
                    {
                        Title = "Матриця",
                        Description = "Хакер дізнається, що світ, у якому він живе, — це комп’ютерна симуляція.",
                        Genre = "Наукова фантастика",
                        Rating = 8.7m,
                        TicketPrice = 240.00m,
                        DurationMinutes = 136,
                        ReleaseDate = new DateTime(1999, 3, 31)
                    },
                    new Film
                    {
                        Title = "Коко",
                        Description = "Хлопчик Міґель вирушає у Країну Мертвих, щоб знайти справжнє значення сім'ї.",
                        Genre = "Мультфільм",
                        Rating = 8.4m,
                        TicketPrice = 180.00m,
                        DurationMinutes = 105,
                        ReleaseDate = new DateTime(2017, 11, 22)
                    },
                    new Film
                    {
                        Title = "Дюна",
                        Description = "Молодий спадкоємець вирушає на небезпечну планету Арракіс, щоб захистити свій народ.",
                        Genre = "Фантастика",
                        Rating = 8.2m,
                        TicketPrice = 280.00m,
                        DurationMinutes = 155,
                        ReleaseDate = new DateTime(2021, 10, 22)
                    }
                );
                context.SaveChanges();
            }
            if (!context.Screenings.Any())
            {
                var films = context.Films.ToList();
                var startDate = new DateTime(2025, 12, 1);

                var halls = new[] { "Зал 1", "Зал 2", "Зал 3" };
                int hallIndex = 0;

                List<Screening> screenings = new();

                foreach (var film in films)
                {
                    var screening1 = new Screening
                    {
                        FilmId = film.FilmID.Value,
                        StartTime = startDate.AddHours(14),
                        Hall = halls[hallIndex % halls.Length],
                        IsOver = false
                    };

                    var screening2 = new Screening
                    {
                        FilmId = film.FilmID.Value,
                        StartTime = startDate.AddHours(19),
                        Hall = halls[(hallIndex + 1) % halls.Length],
                        IsOver = false
                    };

                    hallIndex++;

                    screenings.Add(screening1);
                    screenings.Add(screening2);
                }

                context.Screenings.AddRange(screenings);
                context.SaveChanges();

                var rows = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                var allScreenings = context.Screenings.ToList();
                List<Seat> seats = new();

                foreach (var scr in allScreenings)
                {
                    foreach (var row in rows)
                    {
                        for (int num = 1; num <= 10; num++)
                        {
                            seats.Add(new Seat
                            {
                                ScreeningId = scr.Id,
                                Row = row,
                                Number = num,
                                IsBooked = false
                            });
                        }
                    }
                }

                context.Seats.AddRange(seats);
                context.SaveChanges();
            }
        }
    }
}