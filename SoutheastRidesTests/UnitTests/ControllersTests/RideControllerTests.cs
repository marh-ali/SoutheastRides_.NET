using Microsoft.AspNetCore.Mvc;
using Moq;
using SoutheastRides.DTO;
using Tests.Common;
using Xunit;

public class RideControllerTests
{
    private readonly RideController _controller;
    private readonly Mock<IRideService> _mockRideService;

    public RideControllerTests()
    {
        _mockRideService = new Mock<IRideService>();
        _controller = new RideController(_mockRideService.Object);
    }


    //-------------- HAPPY CASES --------------//

    [Fact]
    public async Task GetAllRides_ReturnsOk_WithRides()
    {
        // Arrange
        var rides = TestsHelper.CreateMockRides();
        _mockRideService.Setup(s => s.GetAllRides()).ReturnsAsync(rides);

        // Act
        var result = await _controller.GetAllRides();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Ride>>(okResult.Value);
        Assert.Equal(rides.Count, returnValue.Count);
    }

    [Fact]
    public async Task GetRideById_ReturnsRide_WhenIdExists()
    {
        // Arrange
        var ride = TestsHelper.CreateMockRide();
        _mockRideService.Setup(s => s.GetRide(ride.Id)).ReturnsAsync(ride);

        // Act
        var result = await _controller.GetRideById(ride.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Ride>(okResult.Value);
        Assert.Equal(ride.Id, returnValue.Id);
    }

    [Fact]
    public async Task CreateRide_ReturnsCreatedRide()
    {
        // Arrange
        var newRide = TestsHelper.CreateMockRide();
        _mockRideService.Setup(s => s.CreateRide(newRide)).ReturnsAsync(newRide);

        // Act
        var result = await _controller.CreateRide(newRide);

        // Assert
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var returnValue = Assert.IsType<Ride>(createdAtRouteResult.Value);
        Assert.Equal(newRide.Id, returnValue.Id);
    }

    [Fact]
    public async Task UpdateRide_ReturnsRide_WhenUpdateSucceeds()
    {
        // Arrange
        var existingRide = TestsHelper.CreateMockRide();
        var updatedRideDTO = new RideUpdateDTO { Title = "UpdatedRide" };
        _mockRideService.Setup(s => s.GetRide(existingRide.Id)).ReturnsAsync(existingRide);

        // Act
        var result = await _controller.UpdateRide(existingRide.Id, updatedRideDTO);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Ride>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task DeleteRide_ReturnsNoContent_WhenIdExists()
    {
        // Arrange
        var ride = TestsHelper.CreateMockRide();
        _mockRideService.Setup(s => s.GetRide(ride.Id)).ReturnsAsync(ride);

        // Act
        var result = await _controller.DeleteRide(ride.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    //-------------- FAILURE CASES --------------//

    [Fact]
    public async Task GetAllRides_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _mockRideService.Setup(s => s.GetAllRides()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAllRides();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetRideById_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _mockRideService.Setup(s => s.GetRide(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetRideById(TestsHelper.GenerateRandomHexadecimalString());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateRide_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var newRide = TestsHelper.CreateMockRide();
        _mockRideService.Setup(s => s.CreateRide(newRide)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.CreateRide(newRide);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateRide_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var rideId = TestsHelper.GenerateRandomHexadecimalString();
        var updatedRideDTO = new RideUpdateDTO { Title = "UpdatedRide" };
        _mockRideService.Setup(s => s.GetRide(rideId)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var actionResult = await _controller.UpdateRide(rideId, updatedRideDTO);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task DeleteRide_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var rideId = TestsHelper.GenerateRandomHexadecimalString();
        _mockRideService.Setup(s => s.GetRide(rideId)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.DeleteRide(rideId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}