using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(Guid id);
        Task DeleteMovieAsync(Guid id);
        Task AddMovieAsync(Movie movie);
    }
}