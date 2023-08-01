using BikeApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SoutheastRides.Models;

public class SoutheastRidesContext
{
    private readonly IMongoDatabase _database = null;


    public SoutheastRidesContext(MongoClient client, string databaseName)
    {
        _database = client.GetDatabase(databaseName);
    }


    public IMongoCollection<User> Users
    {
        get
        {
            return _database.GetCollection<User>("User");
        }
    }

    public IMongoCollection<Ride> Rides
    {
        get
        {
            return _database.GetCollection<Ride>("Ride");
        }
    }

    public IMongoCollection<Rsvp> Rsvps
    {
        get
        {
            return _database.GetCollection<Rsvp>("Rsvp");
        }
    }
}
