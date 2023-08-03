using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RsvpRepository : IRsvpRepository
{
    private readonly IMongoCollection<Rsvp> _rsvps;

    public RsvpRepository(IMongoDatabase database)
    {
        _rsvps = database.GetCollection<Rsvp>("Rsvp");
    }

    public async Task<IEnumerable<Rsvp>> GetAll()
    {
        return await _rsvps.Find(rsvp => true).ToListAsync();
    }

    public async Task<Rsvp> Get(string id)
    {
        return await _rsvps.Find<Rsvp>(rsvp => rsvp.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Rsvp> Create(Rsvp rsvp)
    {
        await _rsvps.InsertOneAsync(rsvp);
        return rsvp;
    }

    public async Task Update(string id, Rsvp rsvp)
    {
        var filter = Builders<Rsvp>.Filter.Eq("Id", id);
        var updateDefinition = Builders<Rsvp>.Update
            .Set("RsvpStatus", rsvp.RsvpStatus)
            .Set("CyclingExperience", rsvp.CyclingExperience)
            .Set("Comment", rsvp.Comment);

        await _rsvps.UpdateOneAsync(filter, updateDefinition);
    }

    public async Task Delete(string id)
    {
        await _rsvps.DeleteOneAsync(rsvp => rsvp.Id == id);
    }
}
