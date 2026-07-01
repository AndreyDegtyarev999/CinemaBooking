using CinemaBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBooking.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync(); //Получить все фильмы
        Task<Movie?> GetByIdAsync(Guid id);// Получить фильм по ID
        Task AddAsync(Movie movie);// Добавить новый фильм
        Task UpdateAsync(Movie movie);// Обновить существующий фильм
        Task DeleteAsync(Guid id);// Удалить фильм по ID
    }
}
