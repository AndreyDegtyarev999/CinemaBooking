using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

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

        private void OnAddBookingClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Открыть форму создания бронирования");
        }
    }
}