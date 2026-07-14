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

                // Добавляем тестовые фильмы, если база пустая
                if (!context.Movies.Any())
                {
                    var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();

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
            }

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}