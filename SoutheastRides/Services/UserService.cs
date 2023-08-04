using SoutheastRides.Models;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Create(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "The provided user data cannot be null.");

        try
        {
            await _userRepository.Create(user);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the user: {ex.Message}");
        }
    }


    public async Task<User> Get(string id)
    {
        try
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new Exception($"The user with ID: {id} does not exist.");
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while fetching the user: {ex.Message}");
        }
    }

    public async Task Update(string id, User userIn)
    {
        try
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new Exception($"The user with ID: {id} does not exist. Cannot perform update operation.");

            await _userRepository.Update(id, userIn);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the user: {ex.Message}");
        }
    }

    public async Task Remove(string id)
    {
        try
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new Exception($"The user with ID: {id} does not exist. Cannot perform delete operation.");

            await _userRepository.Remove(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the user: {ex.Message}");
        }
    }
}
