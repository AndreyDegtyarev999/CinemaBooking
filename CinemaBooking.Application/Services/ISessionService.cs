using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public interface ISessionService
    {
        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task<Session?> GetSessionByIdAsync(Guid id);
        Task<IEnumerable<Session>> GetSessionsByMovieAsync(Guid movieId);
        Task<IEnumerable<Session>> GetSessionsByDateAsync(DateTime date);
        Task AddSessionAsync(Session session);
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}