using SoutheastRides.Models;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> CreateUser(User user);
    Task<User> GetUser(string id);
    Task UpdateUser(string id, User userIn);
    Task DeleteUser(string id);
}
