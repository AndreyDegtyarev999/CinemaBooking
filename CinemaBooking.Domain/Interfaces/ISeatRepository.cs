using CinemaBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllAsync();
        Task<Seat?> GetByIdAsync(Guid id);
        Task AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(Guid id);
    }
}
