using Microsoft.AspNetCore.Mvc;
using SoutheastRides.DTO;
using SoutheastRides.Models;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching all users: {ex.Message}" });
        }
    }

    [HttpGet("{id}", Name = "GetUser")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        try
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching the user: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User newUser)
    {
        try
        {
            var user = await _userService.CreateUser(newUser);
            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while creating the user: {ex.Message}" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateUser(string id, [FromBody] UpdateUserDTO updatedUserDTO)
    {
        try
        {
            var existingUser = await _userService.GetUser(id);

            if (updatedUserDTO.Username != null)
            {
                existingUser.Username = updatedUserDTO.Username;
            }

            await _userService.UpdateUser(id, existingUser);

            return Ok(existingUser);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while updating the user: {ex.Message}" });
        }
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _userService.GetUser(id);
            await _userService.DeleteUser(user.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while deleting the user: {ex.Message}" });
        }
    }
}
