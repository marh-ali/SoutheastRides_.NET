using MongoDB.Driver;
using Xunit;
using Mongo2Go;
using Tests.Common;

public class RsvpRepositoryTests : IDisposable
{
    private readonly MongoDbRunner _mongoDbRunner;
    private readonly IMongoDatabase _database;
    private readonly RsvpRepository _rsvpRepository;

    public RsvpRepositoryTests()
    {
        _mongoDbRunner = MongoDbRunner.Start();

        var client = new MongoClient(_mongoDbRunner.ConnectionString);
        _database = client.GetDatabase("test");

        var context = new SoutheastRidesContext(client, "test");
        _rsvpRepository = new RsvpRepository(context);
    }

    public void Dispose()
    {
        _mongoDbRunner.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsAllRsvps()
    {
        // Arrange.
        var id_1 = TestsHelper.GenerateRandomHexadecimalString();
        var id_2 = TestsHelper.GenerateRandomHexadecimalString();
        var rsvps = new List<Rsvp>
        {
            new Rsvp { Id = id_1, RsvpStatus = "Going" },
            new Rsvp { Id = id_2, RsvpStatus = "Maybe" }
        };
        await _database.GetCollection<Rsvp>("Rsvp").InsertManyAsync(rsvps);

        // Act
        var result = await _rsvpRepository.GetAll();

        // Assert
        Assert.Equal(rsvps.Count, result.Count());
    }

    [Fact]
    public async Task Get_ReturnsRsvpById()
    {
        var id = TestsHelper.GenerateRandomHexadecimalString();

        // Arrange
        string rsvpId = id;
        var rsvp = new Rsvp { Id = rsvpId };
        await _database.GetCollection<Rsvp>("Rsvp").InsertOneAsync(rsvp);

        // Act
        var result = await _rsvpRepository.Get(rsvpId);

        // Assert
        Assert.Equal(rsvpId, result.Id);
    }

    [Fact]
    public async Task Create_InsertsRsvp()
    {
        // Arrange
        var rsvp = new Rsvp { RsvpStatus = "Going" };

        // Act
        var result = await _rsvpRepository.Create(rsvp);

        // Assert
        Assert.Equal(rsvp, result);
    }

    [Fact]
    public async Task Update_UpdatesRsvp()
    {
        // Arrange
        var id = TestsHelper.GenerateRandomHexadecimalString();
        string rsvpId = id;
        var rsvp = new Rsvp { Id = rsvpId, RsvpStatus = "Going" };
        await _database.GetCollection<Rsvp>("Rsvp").InsertOneAsync(rsvp);
        rsvp.RsvpStatus = "Maybe";

        // Act
        await _rsvpRepository.Update(rsvpId, rsvp);

        // Assert
        var updatedRsvp = await _rsvpRepository.Get(rsvpId);
        Assert.NotNull(updatedRsvp);
        Assert.Equal("Maybe", updatedRsvp.RsvpStatus);
    }

    [Fact]
    public async Task Delete_DeletesRsvp()
    {
        // Arrange
        var id = TestsHelper.GenerateRandomHexadecimalString();
        string rsvpId = id;
        var rsvp = new Rsvp { Id = rsvpId };
        await _database.GetCollection<Rsvp>("Rsvp").InsertOneAsync(rsvp);

        // Act
        await _rsvpRepository.Delete(rsvpId);

        // Assert
        var removedRsvp = await _rsvpRepository.Get(rsvpId);
        Assert.Null(removedRsvp);
    }
}
