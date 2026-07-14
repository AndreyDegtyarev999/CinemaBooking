using Avalonia.Controls;
using Avalonia.Interactivity;
using CinemaBooking.Desktop.Views;

namespace CinemaBooking.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMoviesClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new MoviesView();
        }

        private void OnHallsClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HallsView();
        }

        private void OnSessionsClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SessionsView();
        }

        private void OnBookingsClick(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TextBlock
            {
                Text = "Страница бронирований (в разработке)",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,  // ← ИСПРАВЛЕНО!
                FontSize = 24
            };
        }
    }
}