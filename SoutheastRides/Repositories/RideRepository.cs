using MongoDB.Driver;

public class RideRepository : IRideRepository
{
    private readonly IMongoCollection<Ride> _rides;

    public RideRepository(SoutheastRidesContext context)
    {
        _rides = context.Rides;
    }


public async Task<IEnumerable<Ride>> GetAll()
    {
        return await _rides.Find(ride => true).ToListAsync();
    }

    public async Task<Ride> Get(string id)
    {
        return await _rides.Find(ride => ride.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Ride> Create(Ride ride)
    {
        await _rides.InsertOneAsync(ride);
        return ride;
    }

    public async Task Update(string id, Ride ride)
    {
        var filter = Builders<Ride>.Filter.Eq("Id", id);
        var updateDefinition = Builders<Ride>.Update
            .Set("CreatorId", ride.CreatorId)
            .Set("Title", ride.Title)
            .Set("Description", ride.Description)
            .Set("StartLocation", ride.StartLocation)
            .Set("EndLocation", ride.EndLocation)
            .Set("StartTime", ride.StartTime)
            .Set("EndTime", ride.EndTime)
            .Set("Distance", ride.Distance)
            .Set("MaxParticipants", ride.MaxParticipants)
            .Set("Status", ride.Status);

        await _rides.UpdateOneAsync(filter, updateDefinition);
    }


    public async Task Delete(string id)
    {
        await _rides.DeleteOneAsync(ride => ride.Id == id);
    }
}
