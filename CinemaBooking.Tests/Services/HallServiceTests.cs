using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CinemaBooking.Tests.Services
{
    public class HallServiceTests
    {
        private readonly Mock<IHallRepository> _hallRepositoryMock;
        private readonly HallService _hallService;

        public HallServiceTests()
        {
            _hallRepositoryMock = new Mock<IHallRepository>();
            _hallService = new HallService(_hallRepositoryMock.Object);
        }

        [Fact]
        public async Task AddHallAsync_WithValidHall_ShouldAddHall()
        {
            // Arrange
            var hall = new Hall
            {
                Id = Guid.NewGuid(),
                Name = "Test Hall"
            };

            _hallRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Hall>()))
            .Returns(Task.CompletedTask);

            // Act
            await _hallService.AddHallAsync(hall);

            // Assert
            _hallRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Hall>()), Times.Once);
        }

        [Fact]
        public async Task AddHallAsync_WithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            var hall = new Hall
            {
                Id = Guid.NewGuid(),
                Name = ""
            };

            // Act
            var act = () => _hallService.AddHallAsync(hall);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Название зала не может быть пустым");
        }

        [Fact]
        public async Task GetAllHallsAsync_ShouldReturnAllHalls()
        {
            // Arrange
            var halls = new List<Hall>
            {
                new Hall { Id = Guid.NewGuid(), Name = "Hall 1" },
                new Hall { Id = Guid.NewGuid(), Name = "Hall 2" }
            };

            _hallRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(halls);

            // Act
            var result = await _hallService.GetAllHallsAsync();

            // Assert
            result.Should().HaveCount(2);
        }
    }
}