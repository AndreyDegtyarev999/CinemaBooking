using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;
using CinemaBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly AppDbContext _context;

        public HallRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hall>> GetAllAsync()
        {
            return await _context.Halls.ToListAsync();
        }

        public async Task<Hall?> GetByIdAsync(Guid id)
        {
            return await _context.Halls.FindAsync(id);
        }

        public async Task AddAsync(Hall hall)
        {
            await _context.Halls.AddAsync(hall);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hall hall)
        {
            _context.Halls.Update(hall);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                _context.Halls.Remove(hall);
                await _context.SaveChangesAsync();
            }
        }
    }
}