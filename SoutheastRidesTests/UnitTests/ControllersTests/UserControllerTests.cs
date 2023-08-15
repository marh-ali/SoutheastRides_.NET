using Microsoft.AspNetCore.Mvc;
using Moq;
using SoutheastRides.Models;
using Xunit;
using Tests.Common;
using SoutheastRides.DTO;

public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }


        //-------------- HAPPY CASES --------------//

        [Fact]
        public async Task GetAllUsers_ReturnsOk_WithUsers()
        {
            // Arrange
            var users = TestsHelper.CreateMockUsers();
            _mockUserService.Setup(s => s.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(users.Count, returnValue.Count);
        }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenIdExists()
    {
        // Arrange
        var user = TestsHelper.CreateMockUser();
        _mockUserService.Setup(s => s.GetUser(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(user.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<User>(okResult.Value); Assert.Equal(user.Id, returnValue.Id);
        Assert.Equal(user.Id, returnValue.Id);

    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        var newUser = TestsHelper.CreateMockUser();
        _mockUserService.Setup(s => s.CreateUser(newUser)).ReturnsAsync(newUser);

        // Act
        var result = await _controller.CreateUser(newUser);

        // Assert
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var returnValue = Assert.IsType<User>(createdAtRouteResult.Value);
        Assert.Equal(newUser.Id, returnValue.Id);
    }

    [Fact]
    public async Task UpdateUser_ReturnsUser_WhenUpdateSucceeds()
    {
        // Arrange
        var existingUser = TestsHelper.CreateMockUser();
        var updatedUserDTO = new UpdateUserDTO { Username = "UpdatedUser" };
        _mockUserService.Setup(s => s.GetUser(existingUser.Id)).ReturnsAsync(existingUser);

        // Act
        var result = await _controller.UpdateUser(existingUser.Id, updatedUserDTO);

        // Assert
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_WhenIdExists()
    {
        // Arrange
        var user = TestsHelper.CreateMockUser();
        _mockUserService.Setup(s => s.GetUser(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _controller.DeleteUser(user.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }


    //-------------- FAILURE CASES --------------//

    [Fact]
    public async Task GetAllUsers_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetAllUsers()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUser(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetUserById(TestsHelper.GenerateRandomHexadecimalString());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var newUser = TestsHelper.CreateMockUser();
        _mockUserService.Setup(s => s.CreateUser(newUser)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.CreateUser(newUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var userId = TestsHelper.GenerateRandomHexadecimalString();
        var updatedUserDTO = new UpdateUserDTO { Username = "UpdatedUser" };
        _mockUserService.Setup(s => s.GetUser(userId)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var actionResult = await _controller.UpdateUser(userId, updatedUserDTO);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }



    [Fact]
    public async Task DeleteUser_ReturnsBadRequest_WhenExceptionThrown()
    {
        // Arrange
        var userId = TestsHelper.GenerateRandomHexadecimalString();
        _mockUserService.Setup(s => s.GetUser(userId)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

}
