using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CinemaBooking.Desktop.Views
{
    public partial class AddHallView : Window
    {
        public AddHallView()
        {
            InitializeComponent();
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                ShowError("Введите название зала");
                return;
            }

            try
            {
                var serviceProvider = App.ServiceProvider;
                var hallService = serviceProvider.GetRequiredService<IHallService>();

                var hall = new Hall
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };

                await hallService.AddHallAsync(hall);

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