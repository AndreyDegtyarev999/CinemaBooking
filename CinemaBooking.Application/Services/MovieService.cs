using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;

namespace CinemaBooking.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _movieRepository.GetAllAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task AddMovieAsync(Movie movie)
        {
            // Валидация
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            if (string.IsNullOrWhiteSpace(movie.Title))
                throw new ArgumentException("Название фильма не может быть пустым");

            if (movie.Duration <= 0)
                throw new ArgumentException("Длительность фильма должна быть больше 0");

            await _movieRepository.AddAsync(movie);
        }
        public async Task DeleteMovieAsync(Guid id)
        {
            await _movieRepository.DeleteAsync(id);
        }
    }
}