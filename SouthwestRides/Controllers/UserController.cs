using Microsoft.AspNetCore.Mvc;
using SouthwestRides.Models;

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
    public async Task<IActionResult> UpdateUser(string id, User updatedUser)
    {
        try
        {
            if (await _userService.Get(id) == null)
                return NotFound(new { message = "User not found, unable to update" });
            await _userService.Update(id, updatedUser);
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
