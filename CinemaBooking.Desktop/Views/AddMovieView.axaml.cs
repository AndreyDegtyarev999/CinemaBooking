using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CinemaBooking.Desktop.Views
{
    public partial class AddMovieView : Window
    {
        public AddMovieView()
        {
            InitializeComponent();
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text?.Trim();
            var genre = GenreTextBox.Text?.Trim();
            var description = DescriptionTextBox.Text?.Trim();
            var durationText = DurationTextBox.Text?.Trim();
            var releaseDate = ReleaseDatePicker.SelectedDate;

            // Валидация
            if (string.IsNullOrWhiteSpace(title))
            {
                ShowError("Введите название фильма");
                return;
            }

            if (string.IsNullOrWhiteSpace(durationText) || !int.TryParse(durationText, out int duration) || duration <= 0)
            {
                ShowError("Введите корректную длительность (число больше 0)");
                return;
            }

            try
            {
                var serviceProvider = App.ServiceProvider;
                var movieService = serviceProvider.GetRequiredService<IMovieService>();

                var movie = new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Genre = genre ?? string.Empty,
                    Description = description ?? string.Empty,
                    Duration = duration,
                    ReleaseDate = releaseDate?.Date ?? DateTime.Today
                };

                await movieService.AddMovieAsync(movie);

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