using MongoDB.Driver;
using SoutheastRides.Models;
using Xunit;
using Mongo2Go;
using Tests.Common;

public class UserRepositoryTests : IDisposable
{
    private readonly MongoDbRunner _mongoDbRunner;
    private readonly IMongoDatabase _database;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _mongoDbRunner = MongoDbRunner.Start();

        var client = new MongoClient(_mongoDbRunner.ConnectionString);
        _database = client.GetDatabase("test");

        var context = new SoutheastRidesContext(client, "test");
        _userRepository = new UserRepository(context);
    }

    public void Dispose()
    {
        // Stop the in-memory MongoDB instance when the test is done
        _mongoDbRunner.Dispose();
    }

    [Fact]
    public async Task Get_ReturnsAllUsers()
    {
        // Arrange
        var id_1 = TestsHelper.GenerateRandomHexadecimalString();
        var id_2 = TestsHelper.GenerateRandomHexadecimalString();
        var users = new List<User>
        {
            new User { Id = "64c90b07c027ca9c59035632", Username = "User1" },
            new User { Id = "64c90b07c027ca9c59035631", Username = "User2" }
        };
        await _database.GetCollection<User>("User").InsertManyAsync(users);

        // Act
        var result = await _userRepository.GetAll();

        // Assert
        Assert.Equal(users.Count, result.Count());
    }

    [Fact]
    public async Task Get_ReturnsUserById()
    {
        var id = TestsHelper.GenerateRandomHexadecimalString();
        // Arrange
        string userId = id;
        var user = new User { Id = userId };
        await _database.GetCollection<User>("User").InsertOneAsync(user);

        // Act
        var result = await _userRepository.Get(userId);

        // Assert
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task Create_InsertsUser()
    {
        // Arrange
        var user = new User { Username = "TestUser" };

        // Act
        var result = await _userRepository.Create(user);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task Update_UpdatesUser()
    {
        // Arrange
        var id = TestsHelper.GenerateRandomHexadecimalString();
        string userId = id;
        var user = new User { Id = userId, Username = "OldUsername" };
        await _database.GetCollection<User>("User").InsertOneAsync(user);
        user.Username = "NewUsername";

        // Act
        await _userRepository.Update(userId, user);

        // Assert
        var updatedUser = await _userRepository.Get(userId);
        Assert.NotNull(updatedUser);
        Assert.Equal("NewUsername", updatedUser.Username); 

    }

    [Fact]
    public async Task Remove_RemovesUser()
    {
        // Arrange
        var id = TestsHelper.GenerateRandomHexadecimalString();
        string userId = id;
        var user = new User { Id = userId };
        await _database.GetCollection<User>("User").InsertOneAsync(user);

        // Act
        await _userRepository.Remove(userId);

        // Assert
        var removedUser = await _userRepository.Get(userId);
        Assert.Null(removedUser);
    }
}
