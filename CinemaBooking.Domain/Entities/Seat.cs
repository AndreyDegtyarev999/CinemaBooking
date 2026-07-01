using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CinemaBooking.Domain.Enums;

namespace CinemaBooking.Domain.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public SeatType SeatType { get; set; }
        public Guid HallId { get; set; }

        // Навигационные свойства
        public Hall Hall { get; set; }
    }
}