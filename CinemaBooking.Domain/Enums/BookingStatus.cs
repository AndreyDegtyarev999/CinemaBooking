using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Enums
{
        public enum BookingStatus
        {
            Pending = 0,    // Забронировано, но не оплачено
            Paid = 1,       // Оплачено
            Expired = 2,    // Истекло время брони
            Cancelled = 3,  // Отменено
            Refunded = 4,   // Возврат средств
            Used = 5        // Билет использован
    }
}
