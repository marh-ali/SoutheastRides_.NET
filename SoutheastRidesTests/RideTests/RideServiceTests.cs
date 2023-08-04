using Moq;
using SoutheastRides.Models;
using SoutheastRides.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Common;
using Xunit;

namespace Tests
{
    public class RideServiceTests
    {
        private readonly Mock<IRideRepository> _mockRideRepository;
        private readonly RideService _rideService;

        public RideServiceTests()
        {
            _mockRideRepository = new Mock<IRideRepository>();
            _rideService = new RideService(_mockRideRepository.Object);
        }

        [Fact]
        public async Task GetAllRides_ReturnsEmptyList_WhenNoRides()
        {
            // Arrange
            _mockRideRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Ride>());

            // Act
            var result = await _rideService.GetAllRides();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRide_ReturnsNull_WhenIdIsNull()
        {
            // Act
            var result = await _rideService.GetRide(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateRide_ReturnsRide_WhenRideIsValid()
        {
            // Arrange
            var ride = TestsHelper.CreateMockRide();
            _mockRideRepository.Setup(repo => repo.Create(ride)).ReturnsAsync(ride);

            // Act
            var result = await _rideService.CreateRide(ride);

            // Assert
            Assert.Equal(ride, result);
        }

        [Fact]
        public async Task CreateRide_ThrowsException_WhenRideIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _rideService.CreateRide(null));
        }

        [Fact]
        public async Task UpdateRide_CallsRepository_WhenRideExists()
        {
            // Arrange
            var ride = TestsHelper.CreateMockRide();
            _mockRideRepository.Setup(repo => repo.Update(ride.Id, ride)).Returns(Task.CompletedTask);

            // Act
            await _rideService.UpdateRide(ride.Id, ride);

            // Assert
            _mockRideRepository.Verify(repo => repo.Update(ride.Id, ride), Times.Once);
        }

        [Fact]
        public async Task UpdateRide_ThrowsException_WhenIdIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _rideService.UpdateRide(null, TestsHelper.CreateMockRide()));
        }

        [Fact]
        public async Task DeleteRide_CallsRepository_WhenIdIsValid()
        {
            // Arrange
            var id = "validId";
            _mockRideRepository.Setup(repo => repo.Delete(id)).Returns(Task.CompletedTask);

            // Act
            await _rideService.DeleteRide(id);

            // Assert
            _mockRideRepository.Verify(repo => repo.Delete(id), Times.Once);
        }

        [Fact]
        public async Task DeleteRide_ThrowsException_WhenIdIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _rideService.DeleteRide(null));
        }
    }
}
