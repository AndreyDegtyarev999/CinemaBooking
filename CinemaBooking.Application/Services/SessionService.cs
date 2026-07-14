using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;

namespace CinemaBooking.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await _sessionRepository.GetAllAsync();
        }

        public async Task<Session?> GetSessionByIdAsync(Guid id)
        {
            return await _sessionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Session>> GetSessionsByMovieAsync(Guid movieId)
        {
            var sessions = await _sessionRepository.GetAllAsync();
            return sessions.Where(s => s.MovieId == movieId);
        }

        public async Task<IEnumerable<Session>> GetSessionsByDateAsync(DateTime date)
        {
            var sessions = await _sessionRepository.GetAllAsync();
            return sessions.Where(s => s.StartTime.Date == date.Date);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _sessionRepository.DeleteAsync(id);
        }

        public async Task AddSessionAsync(Session session)
        {
            await _sessionRepository.AddAsync(session);
        }

        public async Task UpdateSessionAsync(Session session)
        {
            await _sessionRepository.UpdateAsync(session);
        }

        public async Task DeleteSessionAsync(Guid id)
        {
            await _sessionRepository.DeleteAsync(id);
        }
    }
}