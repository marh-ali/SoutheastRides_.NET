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

        //-------------- HAPPY CASES --------------//

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers_WhenUsersExist()
        {
            // Arrange
            var users = TestsHelper.CreateMockUsers();
            _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync((IEnumerable<User>)null);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.Empty(result);
        }


        [Fact]
        public async Task GetUser_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Get(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUser(user.Id);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsUsers_WhenUsersExist()
        {
            // Arrange
            var users = TestsHelper.CreateMockUsers();
            _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.Equal(users, result);
        }


        [Fact]
        public async Task CreateUser_ReturnsUser_WhenUserIsValid()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Create(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.CreateUser(user);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UpdateUser_UpdatesUser_WhenUserExists()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Get(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.Update(user.Id, user)).Returns(Task.CompletedTask);

            // Act
            await _userService.UpdateUser(user.Id, user);

            // Assert
            _mockUserRepository.Verify(repo => repo.Update(user.Id, user), Times.Once);
        }

        [Fact]
        public async Task RemoveUser_RemovesUser_WhenUserExists()
        {
            // Arrange
            var user = TestsHelper.CreateMockUser();
            _mockUserRepository.Setup(repo => repo.Get(user.Id)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.Remove(user.Id)).Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUser(user.Id);

            // Assert
            _mockUserRepository.Verify(repo => repo.Remove(user.Id), Times.Once);
        }


        //-------------- FAIULRE CASES --------------//

        [Fact]
        public async Task GetUser_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUser("nonexistent_id"));
        }

        [Fact]
        public async Task GetUser_ThrowsException_WhenUserIdIsInvalid()
        {
            // Arrange
            var invalidId = "short_id"; // Less than 24 characters

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUser(invalidId));
        }

        [Fact]
        public async Task CreateUser_ThrowsException_WhenUserIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateUser(null));
        }


        [Fact]
        public async Task UpdateUser_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateUser("nonexistent_id", TestsHelper.CreateMockUser()));
        }

        [Fact]
        public async Task RemoveUser_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Get("nonexistent_id")).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.DeleteUser("nonexistent_id"));
        }

        [Fact]
        public async Task GetAllUsers_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAll()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetAllUsers());
        }

    }
}
