using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        private async void OnAddMovieClick(object sender, RoutedEventArgs e)
        {
            var addMovieForm = new AddMovieView();
            var result = await addMovieForm.ShowDialog<bool>(this.GetVisualAncestors().OfType<Window>().FirstOrDefault());

            if (result)
            {
                LoadMovies();
            }
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
        private async void OnDeleteMovieClick(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, которая была нажата
            var button = sender as Button;
            if (button == null) return;

            // Получаем фильм из DataContext кнопки
            var movie = button.DataContext as Movie;
            if (movie == null) return;

            // Показываем подтверждение
            var confirmWindow = new Window
            {
                Title = "Подтверждение",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new StackPanel
                {
                    Margin = new Avalonia.Thickness(20),
                    Children =
            {
                new TextBlock
                {
                    Text = $"Вы действительно хотите удалить фильм \"{movie.Title}\"?",
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    Margin = new Avalonia.Thickness(0, 0, 0, 20)
                },
                new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions("* ,10,*"),
                    Children =
                    {
                        new Button
                        {
                            Content = "Удалить",
                            Background = Avalonia.Media.Brushes.Red,
                            Foreground = Avalonia.Media.Brushes.White,
                            [Grid.ColumnProperty] = 0,
                            Tag = true
                        },
                        new Button
                        {
                            Content = "Отмена",
                            [Grid.ColumnProperty] = 2,
                            Tag = false
                        }
                    }
                }
            }
                }
            };

            // Настраиваем обработчики кнопок
            var deleteButton = (confirmWindow.Content as StackPanel).Children[1] as Grid;
            var yesButton = deleteButton.Children[0] as Button;
            var noButton = deleteButton.Children[1] as Button;

            bool? result = null;

            yesButton.Click += (s, args) => { result = true; confirmWindow.Close(); };
            noButton.Click += (s, args) => { result = false; confirmWindow.Close(); };

            var window = this.GetVisualAncestors().OfType<Window>().FirstOrDefault();
            if (window != null)
            {
                await confirmWindow.ShowDialog(window);
            }

            if (result == true)
            {
                try
                {
                    var serviceProvider = App.ServiceProvider;
                    var movieService = serviceProvider.GetRequiredService<IMovieService>();

                    await movieService.DeleteMovieAsync(movie.Id);

                    // Удаляем из списка
                    Movies.Remove(movie);
                    _allMovies.Remove(movie);
                }
                catch (Exception ex)
                {
                    var errorWindow = new Window
                    {
                        Title = "Ошибка",
                        Width = 300,
                        Height = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Content = new StackPanel
                        {
                            Margin = new Avalonia.Thickness(20),
                            Children =
                    {
                        new TextBlock
                        {
                            Text = $"Ошибка при удалении: {ex.Message}",
                            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                            Margin = new Avalonia.Thickness(0, 0, 0, 10)
                        },
                        new Button
                        {
                            Content = "OK",
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        }
                    }
                        }
                    };

                    if (window != null)
                    {
                        await errorWindow.ShowDialog(window);
                    }
                }
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