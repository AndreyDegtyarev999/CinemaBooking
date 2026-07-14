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
            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                throw new ArgumentException("Название фильма не может быть пустым");
            }

            await _movieRepository.AddAsync(movie);
        }
        public async Task DeleteMovieAsync(Guid id)
        {
            await _movieRepository.DeleteAsync(id);
        }
    }
}