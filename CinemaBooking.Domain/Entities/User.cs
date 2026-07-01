using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CinemaBooking.Domain.Enums;

namespace CinemaBooking.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        // Навигационные свойства
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
