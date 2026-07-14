using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CinemaBooking.Desktop.Views
{
    public partial class HallsView : UserControl
    {
        public ObservableCollection<Hall> Halls { get; set; } = new();

        public HallsView()
        {
            InitializeComponent();
            DataContext = this;
            LoadHalls();
        }

        private async void LoadHalls()
        {
            var serviceProvider = App.ServiceProvider;
            var hallService = serviceProvider.GetRequiredService<IHallService>();

            var halls = await hallService.GetAllHallsAsync();

            Halls.Clear();
            foreach (var hall in halls)
            {
                Halls.Add(hall);
            }
        }

        private void OnAddHallClick(object sender, RoutedEventArgs e)
        {
            // Пока просто сообщение - позже создадим форму добавления
            System.Diagnostics.Debug.WriteLine("Открыть форму добавления зала");
        }
    }
}