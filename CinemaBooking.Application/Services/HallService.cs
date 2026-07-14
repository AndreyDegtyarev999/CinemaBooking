using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;

namespace CinemaBooking.Application.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepository _hallRepository;

        public HallService(IHallRepository hallRepository)
        {
            _hallRepository = hallRepository;
        }

        public async Task<IEnumerable<Hall>> GetAllHallsAsync()
        {
            return await _hallRepository.GetAllAsync();
        }

        public async Task<Hall?> GetHallByIdAsync(Guid id)
        {
            return await _hallRepository.GetByIdAsync(id);
        }

        public async Task AddHallAsync(Hall hall)
        {
            if (string.IsNullOrWhiteSpace(hall.Name))
            {
                throw new ArgumentException("Название зала не может быть пустым");
            }
            await _hallRepository.AddAsync(hall);
        }

        public async Task UpdateHallAsync(Hall hall)
        {
            await _hallRepository.UpdateAsync(hall);
        }

        public async Task DeleteHallAsync(Guid id)
        {
            await _hallRepository.DeleteAsync(id);
        }
    }
}