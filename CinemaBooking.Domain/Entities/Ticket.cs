using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string QRCode { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public Guid SeatId { get; set; }
        public Guid BookingId { get; set; }

        // Навигационные свойства
        public User User { get; set; }
        public Session Session { get; set; }
        public Seat Seat { get; set; }
        public Booking Booking { get; set; }
    }
}
