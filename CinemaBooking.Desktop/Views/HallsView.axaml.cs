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
    public partial class HallsView : UserControl
    {
        public ObservableCollection<Hall> Halls { get; set; } = new();

        private List<Hall> _allHalls = new();
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

            _allHalls = (await hallService.GetAllHallsAsync()).ToList();

            Halls.Clear();
            foreach (var hall in _allHalls)
            {
                Halls.Add(hall);
            }
        }

        public void OnSearchClick(object sender, RoutedEventArgs e)  // ← ЭТОТ МЕТОД ДОЛЖЕН БЫТЬ!
        {
            var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
            var searchText = searchTextBox?.Text?.ToLower() ?? string.Empty;

            Halls.Clear();
            var filteredHalls = _allHalls
                .Where(h => h.Name.ToLower().Contains(searchText))
                .ToList();

            foreach (var hall in filteredHalls)
            {
                Halls.Add(hall);
            }
        }

        public void OnShowAllClick(object sender, RoutedEventArgs e)  // ← И ЭТОТ ТОЖЕ!
        {
            Halls.Clear();
            foreach (var hall in _allHalls)
            {
                Halls.Add(hall);
            }

            var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
            if (searchTextBox != null)
            {
                searchTextBox.Text = string.Empty;
            }
        }
        private async void OnDeleteHallClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var hall = button.DataContext as Hall;
            if (hall == null) return;

            try
            {
                var serviceProvider = App.ServiceProvider;
                var hallService = serviceProvider.GetRequiredService<IHallService>();

                await hallService.DeleteAsync(hall.Id);

                Halls.Remove(hall);
                _allHalls.Remove(hall);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }
        private async void OnAddHallClick(object sender, RoutedEventArgs e)
        {
            var addHallForm = new AddHallView();
            var window = this.GetVisualAncestors().OfType<Window>().FirstOrDefault();
            var result = await addHallForm.ShowDialog<bool>(window);

            if (result)
            {
                LoadHalls();
            }
        }
    }
}