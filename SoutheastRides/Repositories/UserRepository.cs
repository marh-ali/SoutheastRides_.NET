using MongoDB.Driver;
using SoutheastRides.Models;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(SoutheastRidesContext context)
    {
        _users = context.Users;
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

    public async Task Update(string id, User userIn)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        var updateDefinition = Builders<User>.Update
            .Set("Username", userIn.Username)
            .Set("Email", userIn.Email)
            .Set("ProfilePictureUrl", userIn.ProfilePictureUrl)
            .Set("AuthProvider", userIn.AuthProvider)
            .Set("AuthProviderId", userIn.AuthProviderId)
            .Set("Bio", userIn.Bio)
            .Set("RideHistory", userIn.RideHistory)
            .Set("FavoriteRoutes", userIn.FavoriteRoutes);

        await _users.UpdateOneAsync(filter, updateDefinition);
    }


    public async Task Remove(User userIn) =>
        await _users.DeleteOneAsync(user => user.Id == userIn.Id);

    public async Task Remove(string id) =>
        await _users.DeleteOneAsync(user => user.Id == id);
}
