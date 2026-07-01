using CinemaBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Interfaces
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllAsync();
        Task<Session?> GetByIdAsync(Guid id);
        Task AddAsync(Session session);
        Task UpdateAsync(Session session);
        Task DeleteAsync(Guid id);
    }
}
