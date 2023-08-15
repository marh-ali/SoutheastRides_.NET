using Microsoft.AspNetCore.Mvc;
using SoutheastRides.DTO;


[ApiController]
[Route("[controller]")]
public class RideController : ControllerBase
{
    private readonly IRideService _rideService;

    public RideController(IRideService rideService)
    {
        _rideService = rideService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ride>>> GetAllRides()
    {
        try
        {
            var rides = await _rideService.GetAllRides();
            return Ok(rides);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching all rides: {ex.Message}" });
        }
    }

    [HttpGet("{id}", Name = "GetRide")]
    public async Task<ActionResult<Ride>> GetRideById(string id)
    {
        try
        {
            var ride = await _rideService.GetRide(id);
            return Ok(ride);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching the ride: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Ride>> CreateRide(Ride newRide)
    {
        try
        {
            var ride = await _rideService.CreateRide(newRide);
            return CreatedAtRoute("GetRide", new { id = ride.Id.ToString() }, ride);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while creating the ride: {ex.Message}" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Ride>> UpdateRide(string id, [FromBody] RideUpdateDTO updatedRideDTO)
    {
        try
        {
            var existingRide = await _rideService.GetRide(id);

            // Only update the properties that are present in the DTO
            if (updatedRideDTO.Title != null)
                existingRide.Title = updatedRideDTO.Title;

            if (updatedRideDTO.Description != null)
                existingRide.Description = updatedRideDTO.Description;

            if (updatedRideDTO.StartLocation != null)
                existingRide.StartLocation = updatedRideDTO.StartLocation;

            if (updatedRideDTO.EndLocation != null)
                existingRide.EndLocation = updatedRideDTO.EndLocation;

            if (updatedRideDTO.StartTime.HasValue)
                existingRide.StartTime = updatedRideDTO.StartTime.Value;

            if (updatedRideDTO.EndTime.HasValue)
                existingRide.EndTime = updatedRideDTO.EndTime.Value;

            if (updatedRideDTO.MaxParticipants.HasValue)
                existingRide.MaxParticipants = updatedRideDTO.MaxParticipants.Value;

            if (updatedRideDTO.Status != null)
                existingRide.Status = updatedRideDTO.Status;

            await _rideService.UpdateRide(id, existingRide);

            return Ok(existingRide);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while updating the ride: {ex.Message}" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRide(string id)
    {
        try
        {
            var ride = await _rideService.GetRide(id);
            await _rideService.DeleteRide(ride.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while deleting the ride: {ex.Message}" });
        }
    }
}
