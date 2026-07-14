using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Enums;
using CinemaBooking.Domain.Interfaces;

namespace CinemaBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Where(b => b.UserId == userId);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            booking.Status = BookingStatus.Pending;
            booking.CreatedAt = DateTime.Now;
            await _bookingRepository.AddAsync(booking);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(Guid id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

        public async Task CancelBookingAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Cancelled;
                await _bookingRepository.UpdateAsync(booking);
            }
        }
    }
}