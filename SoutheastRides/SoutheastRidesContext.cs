using SoutheastRides.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class SoutheastRidesContext : ISoutheastRidesContext
{
    private readonly IMongoDatabase _database = null;


    public SoutheastRidesContext(MongoClient client, string databaseName)
    {
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("User");
    public IMongoCollection<Ride> Rides => _database.GetCollection<Ride>("Ride");
    public IMongoCollection<Rsvp> Rsvps => _database.GetCollection<Rsvp>("Rsvp");
}
