using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CinemaBooking.Domain.Enums;

namespace CinemaBooking.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ExpiresAt { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Навигационные свойства
        public User User { get; set; }
        public Session Session { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
