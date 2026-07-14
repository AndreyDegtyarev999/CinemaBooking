using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(Guid id);
        Task<IEnumerable<Ticket>> GetTicketsByBookingAsync(Guid bookingId);
        Task AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(Guid id);
    }
}