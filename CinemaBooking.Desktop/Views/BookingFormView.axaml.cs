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
    public partial class BookingFormView : Window
    {
        public ObservableCollection<User> Users { get; set; } = new();
        public ObservableCollection<Session> Sessions { get; set; } = new();

        public BookingFormView()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private async void LoadData()
        {
            var serviceProvider = App.ServiceProvider;
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var sessionService = serviceProvider.GetRequiredService<ISessionService>();

            var users = await userService.GetAllUsersAsync();
            var sessions = await sessionService.GetAllSessionsAsync();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }

            Sessions.Clear();
            foreach (var session in sessions)
            {
                Sessions.Add(session);
            }
        }
        private void ShowSuccess(string message)
        {
            var successWindow = new Window
            {
                Title = "Успех",
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
                    Margin = new Avalonia.Thickness(0, 0, 0, 10),
                    Foreground = Avalonia.Media.Brushes.Green
                },
                new Button
                {
                    Content = "OK",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    [!Button.CommandProperty] = new Avalonia.Data.Binding("CloseCommand"),
                    CommandParameter = this
                }
            }
                }
            };

            successWindow.ShowDialog(this);
        }

        private async void OnBookClick(object sender, RoutedEventArgs e)
        {
            var selectedUser = UserListBox.SelectedItem as User;
            var selectedSession = SessionListBox.SelectedItem as Session;

            if (selectedUser == null)
            {
                ShowError("Выберите пользователя");
                return;
            }

            if (selectedSession == null)
            {
                ShowError("Выберите сеанс");
                return;
            }

            try
            {
                var serviceProvider = App.ServiceProvider;
                var bookingService = serviceProvider.GetRequiredService<IBookingService>();

                // Создаём бронирование
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    UserId = selectedUser.Id,
                    SessionId = selectedSession.Id,
                    Status = Domain.Enums.BookingStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                await bookingService.AddBookingAsync(booking);

                ShowSuccess("Бронирование успешно создано!");
                this.Close(true);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при создании бронирования: {ex.Message}");
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
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            [!Button.CommandProperty] = new Avalonia.Data.Binding("CloseCommand"),
                            CommandParameter = this
                        }
                    }
                }
            };

            errorWindow.ShowDialog(this);
        }
    }
}