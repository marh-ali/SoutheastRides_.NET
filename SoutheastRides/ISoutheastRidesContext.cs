using MongoDB.Driver;
using SoutheastRides.Models;

public interface ISoutheastRidesContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<Ride> Rides { get; }
    IMongoCollection<Rsvp> Rsvps { get; }
}
