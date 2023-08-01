using SouthwestRides.Models;

public interface IUserService
{
    Task<User> Create(User user);
    Task<User> Get(string id);
    Task Update(string id, User userIn);
    Task Remove(string id);
}
