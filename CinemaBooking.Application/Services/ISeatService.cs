using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync();
        Task<Seat?> GetSeatByIdAsync(Guid id);
        Task<IEnumerable<Seat>> GetSeatsByHallAsync(Guid hallId);
        Task AddSeatAsync(Seat seat);
        Task UpdateSeatAsync(Seat seat);
        Task DeleteSeatAsync(Guid id);
    }
}