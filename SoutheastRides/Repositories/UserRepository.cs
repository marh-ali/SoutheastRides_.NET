using MongoDB.Driver;
using SoutheastRides.Models;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(ISoutheastRidesDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.UserCollectionName);
    }

    public async Task<IEnumerable<User>> Get() =>
        await _users.Find(user => true).ToListAsync();

    public async Task<User> Get(string id) =>
        await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<User> Create(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task Update(string id, User userIn) =>
        await _users.ReplaceOneAsync(user => user.Id == id, userIn);

    public async Task Remove(User userIn) =>
        await _users.DeleteOneAsync(user => user.Id == userIn.Id);

    public async Task Remove(string id) =>
        await _users.DeleteOneAsync(user => user.Id == id);
}
