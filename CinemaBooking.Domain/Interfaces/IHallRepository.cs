using CinemaBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Interfaces
{
    public interface IHallRepository
    {
        Task<IEnumerable<Hall>> GetAllAsync();
        Task<Hall?> GetByIdAsync(Guid id);
        Task AddAsync(Hall hall);
        Task UpdateAsync(Hall hall);
        Task DeleteAsync(Guid id);
    }
}
