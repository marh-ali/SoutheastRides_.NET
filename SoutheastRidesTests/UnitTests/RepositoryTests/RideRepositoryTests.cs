using MongoDB.Driver;
using SoutheastRides.Models;
using Xunit;
using Mongo2Go;
using Tests.Common;

public class RideRepositoryTests : IDisposable
{
    private readonly MongoDbRunner _mongoDbRunner;
    private readonly IMongoDatabase _database;
    private readonly RideRepository _rideRepository;

    public RideRepositoryTests()
    {
        _mongoDbRunner = MongoDbRunner.Start();

        var client = new MongoClient(_mongoDbRunner.ConnectionString);
        _database = client.GetDatabase("test");

        var context = new SoutheastRidesContext(client, "test");
        _rideRepository = new RideRepository(context);
    }

    public void Dispose()
    {
        _mongoDbRunner.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsAllRides()
    {
        // Arrange
        var id_1 = TestsHelper.GenerateRandomHexadecimalString();
        var id_2 = TestsHelper.GenerateRandomHexadecimalString();
        var rides = new List<Ride>
        {
            new Ride { Id = id_1, Title = "Ride1" },
            new Ride { Id = id_2, Title = "Ride2" }
        };

        await _database.GetCollection<Ride>("Ride").InsertManyAsync(rides);

        // Act
        var result = await _rideRepository.GetAll();

        // Assert
        Assert.Equal(rides.Count, result.Count());
    }

    [Fact]
    public async Task Get_ReturnsRideById()
    {
        var id = TestsHelper.GenerateRandomHexadecimalString();

        // Arrange
        string rideId = id;
        var ride = new Ride { Id = rideId };
        await _database.GetCollection<Ride>("Ride").InsertOneAsync(ride);

        // Act
        var result = await _rideRepository.Get(rideId);

        // Assert
        Assert.Equal(rideId, result.Id);
    }

    [Fact]
    public async Task Create_InsertsRide()
    {
        // Arrange
        var ride = new Ride { Title = "TestRide" };

        // Act
        var result = await _rideRepository.Create(ride);

        // Assert
        Assert.Equal(ride, result);
    }

    [Fact]
    public async Task Update_UpdatesRide()
    {
        var id = TestsHelper.GenerateRandomHexadecimalString();

        // Arrange
        string rideId = id;
        var ride = new Ride { Id = rideId, Title = "OldTitle" };
        await _database.GetCollection<Ride>("Ride").InsertOneAsync(ride);
        ride.Title = "NewTitle";

        // Act
        await _rideRepository.Update(rideId, ride);

        // Assert
        var updatedRide = await _rideRepository.Get(rideId);
        Assert.NotNull(updatedRide);
        Assert.Equal("NewTitle", updatedRide.Title);
    }

    [Fact]
    public async Task Delete_DeletesRide()
    {
        var id = TestsHelper.GenerateRandomHexadecimalString();

        // Arrange
        string rideId = id;
        var ride = new Ride { Id = rideId };
        await _database.GetCollection<Ride>("Ride").InsertOneAsync(ride);

        // Act
        await _rideRepository.Delete(rideId);

        // Assert
        var removedRide = await _rideRepository.Get(rideId);
        Assert.Null(removedRide);
    }
}
