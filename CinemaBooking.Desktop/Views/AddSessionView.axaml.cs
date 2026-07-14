using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CinemaBooking.Desktop.Views
{
    public partial class AddSessionView : Window
    {
        public ObservableCollection<Movie> Movies { get; set; } = new();
        public ObservableCollection<Hall> Halls { get; set; } = new();

        public AddSessionView()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private async void LoadData()
        {
            var serviceProvider = App.ServiceProvider;
            var movieService = serviceProvider.GetRequiredService<IMovieService>();
            var hallService = serviceProvider.GetRequiredService<IHallService>();

            var movies = await movieService.GetAllMoviesAsync();
            var halls = await hallService.GetAllHallsAsync();

            Movies.Clear();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }

            Halls.Clear();
            foreach (var hall in halls)
            {
                Halls.Add(hall);
            }
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            var selectedMovie = MovieListBox.SelectedItem as Movie;
            var selectedHall = HallListBox.SelectedItem as Hall;
            var selectedDate = StartDatePicker.SelectedDate;
            var selectedTime = StartTimePicker.SelectedTime;

            // Валидация
            if (selectedMovie == null)
            {
                ShowError("Выберите фильм");
                return;
            }

            if (selectedHall == null)
            {
                ShowError("Выберите зал");
                return;
            }

            if (selectedDate == null)
            {
                ShowError("Выберите дату");
                return;
            }

            if (selectedTime == null)
            {
                ShowError("Выберите время");
                return;
            }

            try
            {
                var serviceProvider = App.ServiceProvider;
                var sessionService = serviceProvider.GetRequiredService<ISessionService>();

                var startTime = selectedDate.Value.Date.Add(selectedTime.Value);
                var endTime = startTime.AddMinutes(selectedMovie.Duration);

                var session = new Session
                {
                    Id = Guid.NewGuid(),
                    MovieId = selectedMovie.Id,
                    HallId = selectedHall.Id,
                    StartTime = startTime,
                    EndTime = endTime
                };

                await sessionService.AddSessionAsync(session);

                this.Close(true);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при добавлении: {ex.Message}");
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        private void ShowError(string message)
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
                            Text = message,
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

            errorWindow.ShowDialog(this);
        }
    }
}