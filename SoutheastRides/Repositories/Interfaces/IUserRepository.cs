using SoutheastRides.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> Get(string id);
    Task<User> Create(User user);
    Task Update(string id, User userIn);
    Task Remove(string id);
}
