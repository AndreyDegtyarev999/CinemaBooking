using CinemaBooking.Application.Services;
using CinemaBooking.Domain.Entities;
using CinemaBooking.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CinemaBooking.Tests.Services
{
    public class SessionServiceTests
    {
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly SessionService _sessionService;

        public SessionServiceTests()
        {
            _sessionRepositoryMock = new Mock<ISessionRepository>();
            _sessionService = new SessionService(_sessionRepositoryMock.Object);
        }

        [Fact]
        public async Task AddSessionAsync_WithValidSession_ShouldAddSession()
        {
            // Arrange
            var session = new Session
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                HallId = Guid.NewGuid(),
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2)
            };

            _sessionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Session>()))
                .Returns(Task.CompletedTask);

            // Act
            await _sessionService.AddSessionAsync(session);

            // Assert
            _sessionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Session>()), Times.Once);
        }

        [Fact]
        public async Task AddSessionAsync_WithEndTimeBeforeStartTime_ShouldThrowArgumentException()
        {
            // Arrange
            var session = new Session
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                HallId = Guid.NewGuid(),
                StartTime = DateTime.Now.AddDays(1).AddHours(2),
                EndTime = DateTime.Now.AddDays(1)
            };

            // Act
            var act = () => _sessionService.AddSessionAsync(session);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Время начала должно быть раньше времени окончания");
        }

        [Fact]
        public async Task AddSessionAsync_WithPastTime_ShouldThrowArgumentException()
        {
            // Arrange
            var session = new Session
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                HallId = Guid.NewGuid(),
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1)
            };

            // Act
            var act = () => _sessionService.AddSessionAsync(session);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Сеанс не может быть в прошлом");
        }
    }
}