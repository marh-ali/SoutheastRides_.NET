using Microsoft.AspNetCore.Mvc;
using SoutheastRides.DTO;
using SoutheastRides.Models;
using SoutheastRides.Services;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class RsvpController : ControllerBase
{
    private readonly IRsvpService _rsvpService;
    private readonly IUserService _userService;

    public RsvpController(IRsvpService rsvpService, IUserService userService)
    {
        _rsvpService = rsvpService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<Rsvp>> CreateRsvp(Rsvp newRsvp)
    {
        try
        {
            var user = await _userService.Get(newRsvp.UserId);
            if (user == null)
                return BadRequest(new { message = "Invalid user ID. RSVP creation failed." });

            var rsvp = await _rsvpService.CreateRsvp(newRsvp);
            return CreatedAtRoute("GetRsvp", new { id = rsvp.Id.ToString() }, rsvp);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while creating the RSVP: {ex.Message}" });
        }
    }

    [HttpGet("{id:length(24)}", Name = "GetRsvp")]
    public async Task<ActionResult<Rsvp>> GetRsvpById(string id)
    {
        try
        {
            var rsvp = await _rsvpService.GetRsvp(id);
            if (rsvp == null) return NotFound(new { message = "RSVP not found" });
            return rsvp;
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching the RSVP: {ex.Message}" });
        }
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateRsvp(string id, [FromBody] RsvpUpdateDTO updatedRsvpDTO)
    {
        try
        {
            var existingRsvp = await _rsvpService.GetRsvp(id);

            if (existingRsvp == null)
                return NotFound(new { message = "RSVP not found, unable to update" });

            if (updatedRsvpDTO.RsvpStatus != null)
                existingRsvp.RsvpStatus = updatedRsvpDTO.RsvpStatus;

            if (updatedRsvpDTO.CyclingExperience != null)
                existingRsvp.CyclingExperience = updatedRsvpDTO.CyclingExperience;

            if (updatedRsvpDTO.Comment != null)
                existingRsvp.Comment = updatedRsvpDTO.Comment;

            await _rsvpService.UpdateRsvp(id, existingRsvp);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while updating the RSVP: {ex.Message}" });
        }
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteRsvp(string id)
    {
        try
        {
            var rsvp = await _rsvpService.GetRsvp(id);
            if (rsvp == null) return NotFound(new { message = "RSVP not found, unable to delete" });
            await _rsvpService.DeleteRsvp(rsvp.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while deleting the RSVP: {ex.Message}" });
        }
    }
}
