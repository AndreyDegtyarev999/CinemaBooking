using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;

namespace CinemaBooking.Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
        {
            return await _seatRepository.GetAllAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(Guid id)
        {
            return await _seatRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Seat>> GetSeatsByHallAsync(Guid hallId)
        {
            var seats = await _seatRepository.GetAllAsync();
            return seats.Where(s => s.HallId == hallId);
        }

        public async Task AddSeatAsync(Seat seat)
        {
            await _seatRepository.AddAsync(seat);
        }

        public async Task UpdateSeatAsync(Seat seat)
        {
            await _seatRepository.UpdateAsync(seat);
        }

        public async Task DeleteSeatAsync(Guid id)
        {
            await _seatRepository.DeleteAsync(id);
        }
    }
}