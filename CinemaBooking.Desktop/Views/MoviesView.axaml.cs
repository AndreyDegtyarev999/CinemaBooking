using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace CinemaBooking.Desktop.Views
{
    public partial class MoviesView : UserControl
    {
        public ObservableCollection<Movie> Movies { get; set; } = new();
        public string SearchText { get; set; } = string.Empty;

        private List<Movie> _allMovies = new();

        public MoviesView()
        {
            InitializeComponent();
            DataContext = this;
            LoadMovies();
        }

        private async void LoadMovies()
        {
            var serviceProvider = App.ServiceProvider;
            var movieService = serviceProvider.GetRequiredService<IMovieService>();

            _allMovies = (await movieService.GetAllMoviesAsync()).ToList();

            Movies.Clear();
            foreach (var movie in _allMovies)
            {
                Movies.Add(movie);
            }
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
            var searchText = searchTextBox?.Text?.ToLower() ?? string.Empty;

            Movies.Clear();
            var filteredMovies = _allMovies
                .Where(m => m.Title.ToLower().Contains(searchText) ||
                           m.Genre.ToLower().Contains(searchText) ||
                           m.Description.ToLower().Contains(searchText))
                .ToList();

            foreach (var movie in filteredMovies)
            {
                Movies.Add(movie);
            }
        }

        private void OnShowAllClick(object sender, RoutedEventArgs e)
        {
            Movies.Clear();
            foreach (var movie in _allMovies)
            {
                Movies.Add(movie);
            }

            var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
            if (searchTextBox != null)
            {
                searchTextBox.Text = string.Empty;
            }
        }
    }
}