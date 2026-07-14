using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId);
        Task AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(Guid id);
        Task CancelBookingAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}