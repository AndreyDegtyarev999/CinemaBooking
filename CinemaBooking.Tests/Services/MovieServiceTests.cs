using System;
using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CinemaBooking.Tests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieService = new MovieService(_movieRepositoryMock.Object);
        }

        [Fact]
        public async Task AddMovieAsync_WithValidMovie_ShouldAddMovie()
        {
            // Arrange
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Test Movie",
                Genre = "Action",
                Description = "Test Description",
                Duration = 120,
                ReleaseDate = DateTime.Today
            };

            _movieRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Movie>()))
            .Returns(Task.CompletedTask);

            // Act
            await _movieService.AddMovieAsync(movie);

            // Assert
            _movieRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public async Task AddMovieAsync_WithNullMovie_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            var act = () => _movieService.AddMovieAsync(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddMovieAsync_WithEmptyTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "",
                Genre = "Action",
                Duration = 120
            };

            // Act
            var act = () => _movieService.AddMovieAsync(movie);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Название фильма не может быть пустым");
        }

        [Fact]
        public async Task AddMovieAsync_WithInvalidDuration_ShouldThrowArgumentException()
        {
            // Arrange
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Test Movie",
                Duration = 0
            };

            // Act
            var act = () => _movieService.AddMovieAsync(movie);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Длительность фильма должна быть больше 0");
        }

        [Fact]
        public async Task GetAllMoviesAsync_ShouldReturnAllMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = Guid.NewGuid(), Title = "Movie 1", Duration = 120 },
                new Movie { Id = Guid.NewGuid(), Title = "Movie 2", Duration = 90 }
            };

            _movieRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(movies);

            // Act
            var result = await _movieService.GetAllMoviesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(m => m.Title == "Movie 1");
        }

        [Fact]
        public async Task DeleteMovieAsync_WithValidId_ShouldDeleteMovie()
        {
            // Arrange
            var movieId = Guid.NewGuid();

            _movieRepositoryMock.Setup(r => r.DeleteAsync(movieId))
            .Returns(Task.CompletedTask);

            // Act
            await _movieService.DeleteMovieAsync(movieId);

            // Assert
            _movieRepositoryMock.Verify(r => r.DeleteAsync(movieId), Times.Once);
        }
    }
}