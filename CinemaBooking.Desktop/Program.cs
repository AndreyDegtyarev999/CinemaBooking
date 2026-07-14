using Avalonia;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;
using CinemaBooking.Infrastructure.Data;
using CinemaBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Desktop
{
    internal class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IHallService, HallService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();

            var serviceProvider = services.BuildServiceProvider();
            App.ServiceProvider = serviceProvider;

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();

                // Объявляем сервисы ОДИН раз в начале
                var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();
                var hallService = scope.ServiceProvider.GetRequiredService<IHallService>();
                var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

                // Добавляем тестовые фильмы, если база пустая
                if (!context.Movies.Any())
                {
                    await movieService.AddMovieAsync(new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = "Начало",
                        Description = "Фантастический триллер Кристофера Нолана о ворах, которые проникают в сны",
                        Duration = 148,
                        ReleaseDate = new DateTime(2010, 7, 16),
                        Genre = "Фантастика, Триллер"
                    });

                    await movieService.AddMovieAsync(new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = "Интерстеллар",
                        Description = "Группа исследователей путешествует через червоточину в космосе",
                        Duration = 169,
                        ReleaseDate = new DateTime(2014, 11, 7),
                        Genre = "Фантастика, Драма"
                    });

                    await movieService.AddMovieAsync(new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = "Тёмный рыцарь",
                        Description = "Бэтмен против Джокера в борьбе за душу Готэма",
                        Duration = 152,
                        ReleaseDate = new DateTime(2008, 7, 18),
                        Genre = "Боевик, Триллер"
                    });
                }

                // Добавляем тестовые залы, если база пустая
                if (!context.Halls.Any())
                {
                    await hallService.AddHallAsync(new Hall
                    {
                        Id = Guid.NewGuid(),
                        Name = "Зал IMAX",
                    });

                    await hallService.AddHallAsync(new Hall
                    {
                        Id = Guid.NewGuid(),
                        Name = "Зал VIP",
                    });

                    await hallService.AddHallAsync(new Hall
                    {
                        Id = Guid.NewGuid(),
                        Name = "Зал 3D",
                    });
                }

                // Добавляем тестовые сеансы, если база пустая
                if (!context.Sessions.Any())
                {
                    var movies = (await movieService.GetAllMoviesAsync()).ToList();
                    var halls = (await hallService.GetAllHallsAsync()).ToList();

                    if (movies.Any() && halls.Any())
                    {
                        // Сеанс 1: Начало в IMAX
                        await sessionService.AddSessionAsync(new Session
                        {
                            Id = Guid.NewGuid(),
                            MovieId = movies[0].Id,
                            HallId = halls[0].Id,
                            StartTime = DateTime.Today.AddHours(18),
                            EndTime = DateTime.Today.AddHours(18).AddMinutes(148),
                        });

                        // Сеанс 2: Интерстеллар в VIP
                        await sessionService.AddSessionAsync(new Session
                        {
                            Id = Guid.NewGuid(),
                            MovieId = movies[1].Id,
                            HallId = halls[1].Id,
                            StartTime = DateTime.Today.AddHours(20),
                            EndTime = DateTime.Today.AddHours(20).AddMinutes(169),
                        });

                        // Сеанс 3: Тёмный рыцарь в 3D
                        await sessionService.AddSessionAsync(new Session
                        {
                            Id = Guid.NewGuid(),
                            MovieId = movies[2].Id,
                            HallId = halls[2].Id,
                            StartTime = DateTime.Today.AddHours(19),
                            EndTime = DateTime.Today.AddHours(19).AddMinutes(152),
                        });
                    }
                }
            }

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}