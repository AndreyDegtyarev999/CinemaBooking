using CinemaBooking.Domain.Entities;

namespace CinemaBooking.Application.Services
{
    public interface IHallService
    {
        Task<IEnumerable<Hall>> GetAllHallsAsync();
        Task<Hall?> GetHallByIdAsync(Guid id);
        Task AddHallAsync(Hall hall);
        Task UpdateHallAsync(Hall hall);
        Task DeleteHallAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}