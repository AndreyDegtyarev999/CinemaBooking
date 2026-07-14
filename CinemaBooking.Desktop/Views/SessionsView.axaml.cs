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
    public partial class SessionsView : UserControl
    {
        public ObservableCollection<Session> Sessions { get; set; } = new();

        private List<Session> _allSessions = new();

        public SessionsView()
        {
            InitializeComponent();
            DataContext = this;
            LoadSessions();
        }
        private async void OnDeleteSessionClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var session = button.DataContext as Session;
            if (session == null) return;

            try
            {
                var serviceProvider = App.ServiceProvider;
                var sessionService = serviceProvider.GetRequiredService<ISessionService>();

                await sessionService.DeleteAsync(session.Id);

                Sessions.Remove(session);
                _allSessions.Remove(session);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }
        private async void LoadSessions()
        {
            var serviceProvider = App.ServiceProvider;
            var sessionService = serviceProvider.GetRequiredService<ISessionService>();

            _allSessions = (await sessionService.GetAllSessionsAsync()).ToList();

            Sessions.Clear();
            foreach (var session in _allSessions)
            {
                Sessions.Add(session);
            }
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
            var searchText = searchTextBox?.Text?.ToLower() ?? string.Empty;

            Sessions.Clear();
            var filteredSessions = _allSessions
                .Where(s => s.Movie.Title.ToLower().Contains(searchText) ||
                           s.Hall.Name.ToLower().Contains(searchText))
                .ToList();

            foreach (var session in filteredSessions)
            {
                Sessions.Add(session);
            }
        }

        private void OnShowAllClick(object sender, RoutedEventArgs e)
        {
            Sessions.Clear();
            foreach (var session in _allSessions)
            {
                Sessions.Add(session);
            }

            var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
            if (searchTextBox != null)
            {
                searchTextBox.Text = string.Empty;
            }
        }

        private async void OnAddSessionClick(object sender, RoutedEventArgs e)
        {
            var addSessionForm = new AddSessionView();
            var window = this.GetVisualAncestors().OfType<Window>().FirstOrDefault();
            var result = await addSessionForm.ShowDialog<bool>(window);

            if (result)
            {
                LoadSessions();
            }
        }
    }
}