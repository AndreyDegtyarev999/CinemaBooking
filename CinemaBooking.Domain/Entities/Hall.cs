using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CinemaBooking.Domain.Entities
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalRows { get; set; }
        public int TotalSeatsPerRow { get; set; }

        // Навигационные свойства
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}