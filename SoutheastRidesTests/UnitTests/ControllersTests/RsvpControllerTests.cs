using Microsoft.AspNetCore.Mvc;
using Moq;
using SoutheastRides.Services;
using Tests.Common;
using Xunit;

public class RsvpControllerTests
{
    private readonly RsvpController _controller;
    private readonly Mock<IRsvpService> _mockRsvpService;
    private readonly Mock<IUserService> _mockUserService;

    public RsvpControllerTests()
    {
        _mockRsvpService = new Mock<IRsvpService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new RsvpController(_mockRsvpService.Object, _mockUserService.Object);
    }

    //-------------- HAPPY CASES --------------//

    [Fact]
    public async Task GetAllRsvps_ReturnsOk_WithRsvps()
    {
        // Arrange
        var rsvps = TestsHelper.CreateMockRsvps();
        _mockRsvpService.Setup(s => s.GetAllRsvps()).ReturnsAsync(rsvps);

        // Act
        var result = await _controller.GetAllRsvps();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Rsvp>>(okResult.Value);
        Assert.Equal(rsvps.Count, returnValue.Count);
    }

    [Fact]
    public async Task GetRsvpById_ReturnsRsvp_WhenIdExists()
    {
        // Arrange
        var rsvp = TestsHelper.CreateMockRsvp();
        _mockRsvpService.Setup(s => s.GetRsvp(rsvp.Id)).ReturnsAsync(rsvp);

        // Act
        var result = await _controller.GetRsvpById(rsvp.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Rsvp>(okResult.Value);
        Assert.Equal(rsvp.Id, returnValue.Id);
    }

    [Fact]
    public async Task CreateRsvp_ReturnsCreatedRsvp()
    {
        // Arrange
        var newRsvp = TestsHelper.CreateMockRsvp();
        _mockRsvpService.Setup(s => s.CreateRsvp(newRsvp)).ReturnsAsync(newRsvp);

        // Act
        var result = await _controller.CreateRsvp(newRsvp);

        // Assert
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var returnValue = Assert.IsType<Rsvp>(createdAtRouteResult.Value);
        Assert.Equal(newRsvp.Id, returnValue.Id);
    }

    [Fact]
    public async Task UpdateRsvp_ReturnsRsvp_WhenUpdateSucceeds()
    {
        // Arrange
        var existingRsvp = TestsHelper.CreateMockRsvp();
        var updatedRsvpDTO = new RsvpUpdateDTO { RsvpStatus = "UpdatedStatus" };
        _mockRsvpService.Setup(s => s.GetRsvp(existingRsvp.Id)).ReturnsAsync(existingRsvp);

        // Act
        var result = await _controller.UpdateRsvp(existingRsvp.Id, updatedRsvpDTO);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Rsvp>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task DeleteRsvp_ReturnsNoContent_WhenIdExists()
    {
        // Arrange
        var rsvp = TestsHelper.CreateMockRsvp();
        _mockRsvpService.Setup(s => s.GetRsvp(rsvp.Id)).ReturnsAsync(rsvp);

        // Act
        var result = await _controller.DeleteRsvp(rsvp.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    //-------------- FAILURE CASES --------------//

    [Fact]
    public async Task GetAllRsvps_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _mockRsvpService.Setup(s => s.GetAllRsvps()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAllRsvps();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetRsvpById_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _mockRsvpService.Setup(s => s.GetRsvp(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetRsvpById(TestsHelper.GenerateRandomHexadecimalString());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateRsvp_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var newRsvp = TestsHelper.CreateMockRsvp();
        _mockRsvpService.Setup(s => s.CreateRsvp(newRsvp)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.CreateRsvp(newRsvp);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateRsvp_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var rsvpId = TestsHelper.GenerateRandomHexadecimalString();
        var updatedRsvpDTO = new RsvpUpdateDTO { RsvpStatus = "UpdatedStatus" };
        _mockRsvpService.Setup(s => s.GetRsvp(rsvpId)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var actionResult = await _controller.UpdateRsvp(rsvpId, updatedRsvpDTO);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task DeleteRsvp_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var rsvpId = TestsHelper.GenerateRandomHexadecimalString();
        _mockRsvpService.Setup(s => s.GetRsvp(rsvpId)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.DeleteRsvp(rsvpId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

}
