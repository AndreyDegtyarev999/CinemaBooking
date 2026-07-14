using Avalonia.Controls;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CinemaBooking.Desktop.Views
{
    public partial class MoviesView : UserControl
    {
        public ObservableCollection<Movie> Movies { get; set; } = new();

        public MoviesView()
        {
            InitializeComponent();  // ← ЭТО ВАЖНО!
            DataContext = this;
            LoadMovies();
        }

        private async void LoadMovies()
        {
            var serviceProvider = App.ServiceProvider;
            var movieService = serviceProvider.GetRequiredService<IMovieService>();

            var movies = await movieService.GetAllMoviesAsync();

            Movies.Clear();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }
    }
}