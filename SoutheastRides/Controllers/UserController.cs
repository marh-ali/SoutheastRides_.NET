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

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User newUser)
    {
        try
        {
            var user = await _userService.Create(newUser);
            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while creating the user: {ex.Message}" });
        }
    }

    [HttpGet("{id:length(24)}", Name = "GetUser")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        try
        {
            var user = await _userService.Get(id);
            if (user == null) return NotFound(new { message = "User not found" });
            return user;
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching the user: {ex.Message}" });
        }
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDTO updatedUserDTO)
    {
        try
        {
            var existingUser = await _userService.Get(id);

            if (existingUser == null)
                return NotFound(new { message = "User not found, unable to update" });

            // Only update the properties that are present in the DTO
            if (updatedUserDTO.Username != null)
            {
                existingUser.Username = updatedUserDTO.Username;
            }

            // Update other properties if needed

            await _userService.Update(id, existingUser);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while updating the user: {ex.Message}" });
        }
    }


    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _userService.Get(id);
            if (user == null) return NotFound(new { message = "User not found, unable to delete" });
            await _userService.Remove(user.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while deleting the user: {ex.Message}" });
        }
    }
}
