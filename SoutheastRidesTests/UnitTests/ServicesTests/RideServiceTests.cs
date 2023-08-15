using Moq;
using System;
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

        //-------------- HAPPY CASES --------------//


        [Fact]
        public async Task GetAllRides_ReturnsAllRides_WhenRidesExist()
        {
            // Arrange
            var rides = TestsHelper.CreateMockRides(); 
            _mockRideRepository.Setup(r => r.GetAll()).ReturnsAsync(rides);

            // Act
            var result = await _rideService.GetAllRides();

            // Assert
            Assert.Equal(rides, result);
        }

        [Fact]
        public async Task GetAllRides_ReturnsEmptyList_WhenNoRidesExist()
        {
            // Arrange
            _mockRideRepository.Setup(r => r.GetAll()).ReturnsAsync((IEnumerable<Ride>)null);

            // Act
            var result = await _rideService.GetAllRides();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRide_ReturnsRide_WhenRideExists()
        {
            // Arrange
            var ride = TestsHelper.CreateMockRide();
            _mockRideRepository.Setup(repo => repo.Get(ride.Id)).ReturnsAsync(ride);

            // Act
            var result = await _rideService.GetRide(ride.Id);

            // Assert
            Assert.Equal(ride, result);
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
        public async Task UpdateRide_UpdatesRide_WhenRideExists()
        {
            // Arrange
            var ride = TestsHelper.CreateMockRide();
            _mockRideRepository.Setup(r => r.Get(ride.Id)).ReturnsAsync(ride);
            _mockRideRepository.Setup(repo => repo.Update(ride.Id, ride)).Returns(Task.CompletedTask);

            // Act
            await _rideService.UpdateRide(ride.Id, ride);

            // Assert
            _mockRideRepository.Verify(repo => repo.Update(ride.Id, ride), Times.Once);
        }

        [Fact]
        public async Task DeleteRide_DeletesRide_WhenRideExists()
        {
            // Arrange
            var ride = TestsHelper.CreateMockRide();
            _mockRideRepository.Setup(r => r.Get(ride.Id)).ReturnsAsync(ride);
            _mockRideRepository.Setup(repo => repo.Delete(ride.Id)).Returns(Task.CompletedTask);

            // Act
            await _rideService.DeleteRide(ride.Id);

            // Assert
            _mockRideRepository.Verify(repo => repo.Delete(ride.Id), Times.Once);
        }

        //-------------- FAILURE CASES --------------//


        [Fact]
        public async Task GetRide_ThrowsException_WhenRideDoesNotExist() 
        {
            // Arrange
            _mockRideRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((Ride)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _rideService.GetRide("nonexistent_id")); 
        }


        [Fact]
        public async Task CreateRide_ThrowsException_WhenRideIsNull() 
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _rideService.CreateRide(null)); 
        }


        [Fact]
        public async Task UpdateRide_ThrowsException_WhenRideDoesNotExist() 
        {
            // Arrange
            _mockRideRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((Ride)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _rideService.UpdateRide("nonexistent_id", TestsHelper.CreateMockRide()));
        }

        [Fact]
        public async Task DeleteRide_ThrowsException_WhenRideDoesNotExist() 
        {
            // Arrange
            _mockRideRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((Ride)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _rideService.DeleteRide("nonexistent_id")); 
        }
    }
}
