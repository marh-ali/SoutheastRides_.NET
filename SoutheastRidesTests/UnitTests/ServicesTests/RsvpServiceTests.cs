using Moq;
using SoutheastRides.Services;
using Tests.Common;
using Xunit;

namespace SoutheastRides.Tests
{
    public class RsvpServiceTests
    {
        private readonly RsvpService _service;
        private readonly Mock<IRsvpRepository> _repositoryMock;

        public RsvpServiceTests()
        {
            _repositoryMock = new Mock<IRsvpRepository>();
            _service = new RsvpService(_repositoryMock.Object);
        }

        //-------------- HAPPY CASES --------------//

        [Fact]
        public async Task GetAllRsvps_ReturnsAllRsvps_WhenRsvpsExist()
        {
            // Arrange
            var rsvps = TestsHelper.CreateMockRsvps(); 
            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(rsvps);

            // Act
            var result = await _service.GetAllRsvps();

            // Assert
            Assert.Equal(rsvps, result);
            TestsHelper.ClearMocks();
        }

        [Fact]
        public async Task GetAllRsvps_ReturnsEmptyList_WhenNoRsvpsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync((IEnumerable<Rsvp>)null);

            // Act
            var result = await _service.GetAllRsvps();

            // Assert
            Assert.Empty(result);
            TestsHelper.ClearMocks();
        }


        [Fact]
        public async Task GetRsvp_ReturnsRsvp_WhenRsvpIdIsValid()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            _repositoryMock.Setup(r => r.Get(rsvp.Id)).ReturnsAsync(rsvp);

            // Act
            var result = await _service.GetRsvp(rsvp.Id);

            // Assert
            Assert.Equal(rsvp, result);
            TestsHelper.ClearMocks();
        }

        [Fact]
        public async Task GetAllRsvps_ReturnsAllRsvps()
        {
            // Arrange
            var rsvps = TestsHelper.CreateMockRsvps();
            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(rsvps);

            // Act
            var result = await _service.GetAllRsvps();

            // Assert
            Assert.Equal(rsvps.Count, result.Count());
            TestsHelper.ClearMocks();
        }


        [Fact]
        public async Task CreateRsvp_ReturnsRsvp_WhenRsvpIsValid()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            _repositoryMock.Setup(r => r.Create(rsvp)).ReturnsAsync(rsvp);

            // Act
            var result = await _service.CreateRsvp(rsvp);

            // Assert
            Assert.Equal(rsvp, result);
            TestsHelper.ClearMocks();
        }

        [Fact]
        public async Task UpdateRsvp_Successful_WhenRsvpExists()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            _repositoryMock.Setup(r => r.Get(rsvp.Id)).ReturnsAsync(rsvp);
            _repositoryMock.Setup(r => r.Update(rsvp.Id, rsvp)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateRsvp(rsvp.Id, rsvp);

            // Assert
            _repositoryMock.Verify(r => r.Update(rsvp.Id, rsvp), Times.Once);
            TestsHelper.ClearMocks();
        }

        [Fact]
        public async Task DeleteRsvp_Successful_WhenRsvpExists()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            _repositoryMock.Setup(r => r.Get(rsvp.Id)).ReturnsAsync(rsvp);
            _repositoryMock.Setup(r => r.Delete(rsvp.Id)).Returns(Task.CompletedTask); 

            // Act
            await _service.DeleteRsvp(rsvp.Id);

            // Assert
            _repositoryMock.Verify(r => r.Delete(rsvp.Id), Times.Once);
            TestsHelper.ClearMocks();
        }


        //-------------- FAIULRE CASES --------------//

        [Fact]
        public async Task GetRsvp_ThrowsException_WhenRsvpDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get("nonexistent_id")).ReturnsAsync((Rsvp)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetRsvp("nonexistent_id"));
            TestsHelper.ClearMocks();
        }

        [Fact]
        public async Task GetRsvp_ThrowsException_WhenRsvpIdIsInvalid()
        {
            // Arrange
            var invalidId = "short_id"; // Less than 24 characters

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetRsvp(invalidId));
            TestsHelper.ClearMocks();
        }

        [Fact]
        public async Task CreateRsvp_ThrowsArgumentNullException_WhenRsvpIsNull()
        {
            // Arrange
            Rsvp nullRsvp = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateRsvp(nullRsvp));
        }

        [Fact]
        public async Task CreateRsvp_ThrowsArgumentException_WhenDetailsAreIncomplete()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            rsvp.UserId = null; 

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRsvp(rsvp));
        }

        [Fact]
        public async Task UpdateRsvp_ThrowsArgumentException_WhenIdIsInvalidLength()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            var invalidId = "short_id";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateRsvp(invalidId, rsvp));
        }

        [Fact]
        public async Task UpdateRsvp_ThrowsException_WhenRsvpIsNonExistent()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            _repositoryMock.Setup(r => r.Get(rsvp.Id)).ReturnsAsync((Rsvp)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateRsvp(rsvp.Id, rsvp));
        }

        [Fact]
        public async Task DeleteRsvp_ThrowsException_WhenRsvpNonExistent()
        {
            // Arrange
            var rsvp = TestsHelper.CreateMockRsvp();
            _repositoryMock.Setup(r => r.Get(rsvp.Id)).ReturnsAsync((Rsvp)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteRsvp(rsvp.Id));
        }
    }
}
