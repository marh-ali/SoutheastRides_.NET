using Moq;
using SoutheastRides.Models;
using Tests.Common;
using Xunit;

namespace Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsUser_WhenUserIsValid()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Create(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.Create(user);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task CreateUser_ThrowsException_WhenUserIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.Create(null));
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Get(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _userService.Get(user.Id);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUser_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.Get("nonexistent_id"));
        }

        [Fact]
        public async Task UpdateUser_UpdatesUser_WhenUserExists()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Get(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.Update(user.Id, user)).Returns(Task.CompletedTask);

            // Act
            await _userService.Update(user.Id, user);

            // Assert
            _mockUserRepository.Verify(repo => repo.Update(user.Id, user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.Update("nonexistent_id", TestsHelper.CreateMockUser()));
        }

        [Fact]
        public async Task RemoveUser_RemovesUser_WhenUserExists()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Get(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.Remove(user.Id)).Returns(Task.CompletedTask);

            // Act
            await _userService.Remove(user.Id);

            // Assert
            _mockUserRepository.Verify(repo => repo.Remove(user.Id), Times.Once);
        }

        [Fact]
        public async Task RemoveUser_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.Remove("nonexistent_id"));
        }
    }
}
