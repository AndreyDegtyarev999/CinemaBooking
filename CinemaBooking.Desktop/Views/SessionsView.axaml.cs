using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CinemaBooking.Desktop.Views
{
    public partial class SessionsView : UserControl
    {
        public ObservableCollection<Session> Sessions { get; set; } = new();

        public SessionsView()
        {
            InitializeComponent();
            DataContext = this;
            LoadSessions();
        }

        private async void LoadSessions()
        {
            var serviceProvider = App.ServiceProvider;
            var sessionService = serviceProvider.GetRequiredService<ISessionService>();

            var sessions = await sessionService.GetAllSessionsAsync();

            Sessions.Clear();
            foreach (var session in sessions)
            {
                Sessions.Add(session);
            }
        }

        private void OnAddSessionClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Открыть форму добавления сеанса");
        }
    }
}