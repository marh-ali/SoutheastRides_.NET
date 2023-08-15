using Microsoft.AspNetCore.Mvc;
using SoutheastRides.Services;


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


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rsvp>>> GetAllRsvps()
    {
        try
        {
            var rsvps = await _rsvpService.GetAllRsvps();
            return Ok(rsvps);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching all rsvps: {ex.Message}" });
        }
    }

    [HttpGet("{id}", Name = "GetRsvp")]
    public async Task<ActionResult<Rsvp>> GetRsvpById(string id)
    {
        try
        {
            var rsvp = await _rsvpService.GetRsvp(id);
            return Ok(rsvp);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching the RSVP: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Rsvp>> CreateRsvp(Rsvp newRsvp)
    {
        try
        {
            var user = await _userService.GetUser(newRsvp.UserId);
            var rsvp = await _rsvpService.CreateRsvp(newRsvp);
            return CreatedAtRoute("GetRsvp", new { id = rsvp.Id.ToString() }, rsvp);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while creating the RSVP: {ex.Message}" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Rsvp>> UpdateRsvp(string id, [FromBody] RsvpUpdateDTO updatedRsvpDTO)
    {
        try
        {
            var existingRsvp = await _rsvpService.GetRsvp(id);

            if (updatedRsvpDTO.RsvpStatus != null)
                existingRsvp.RsvpStatus = updatedRsvpDTO.RsvpStatus;

            if (updatedRsvpDTO.CyclingExperience != null)
                existingRsvp.CyclingExperience = updatedRsvpDTO.CyclingExperience;

            if (updatedRsvpDTO.Comment != null)
                existingRsvp.Comment = updatedRsvpDTO.Comment;

            await _rsvpService.UpdateRsvp(id, existingRsvp);

            return Ok(existingRsvp);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while updating the RSVP: {ex.Message}" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRsvp(string id)
    {
        try
        {
            var rsvp = await _rsvpService.GetRsvp(id);
            await _rsvpService.DeleteRsvp(rsvp.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while deleting the RSVP: {ex.Message}" });
        }
    }
}
