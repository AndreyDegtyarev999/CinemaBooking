using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public Guid HallId { get; set; }
        public Guid MovieId { get; set; }

        // Навигационные свойства
        public Hall Hall { get; set; }
        public Movie Movie { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
