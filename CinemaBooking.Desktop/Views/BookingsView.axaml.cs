using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CinemaBooking.Desktop.Views
{
    public partial class BookingsView : UserControl
    {
        public ObservableCollection<Booking> Bookings { get; set; } = new();

        public BookingsView()
        {
            InitializeComponent();
            DataContext = this;
            LoadBookings();
        }

        private async void LoadBookings()
        {
            var serviceProvider = App.ServiceProvider;
            var bookingService = serviceProvider.GetRequiredService<IBookingService>();

            var bookings = await bookingService.GetAllBookingsAsync();

            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }
        }

        private async void OnAddBookingClick(object sender, RoutedEventArgs e)
        {
            var bookingForm = new BookingFormView();

            // Находим родительское окно
            var window = this.GetVisualAncestors().OfType<Window>().FirstOrDefault();
            if (window == null)
            {
                System.Diagnostics.Debug.WriteLine("Не удалось найти родительское окно");
                return;
            }

            var result = await bookingForm.ShowDialog<bool>(window);

            // Если бронирование создано — перезагружаем список
            if (result)
            {
                LoadBookings();
            }
        }
        private async void OnDeleteBookingClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var booking = button.DataContext as Booking;
            if (booking == null) return;

            try
            {
                var serviceProvider = App.ServiceProvider;
                var bookingService = serviceProvider.GetRequiredService<IBookingService>();

                await bookingService.DeleteAsync(booking.Id);

                Bookings.Remove(booking);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }
    }
}