using SouthwestRides.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> Get();
    Task<User> Get(string id);
    Task<User> Create(User user);
    Task Update(string id, User userIn);
    Task Remove(User userIn);
    Task Remove(string id);
}
