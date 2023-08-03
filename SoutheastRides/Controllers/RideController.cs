using Microsoft.AspNetCore.Mvc;
using SoutheastRides.DTO;
using SoutheastRides.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class RideController : ControllerBase
{
    private readonly IRideService _rideService;
    private readonly IUserService _userService;

    public RideController(IRideService rideService, IUserService userService)
    {
        _rideService = rideService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<Ride>> CreateRide(Ride newRide)
    {
        try
        {
            // Assuming you have a UserService or a UserRepository to check if the creator exists
            var creator = await _userService.Get(newRide.CreatorId);
            if (creator == null)
                return BadRequest(new { message = "Invalid creator ID. Ride creation failed." });

            var ride = await _rideService.CreateRide(newRide);
            return CreatedAtRoute("GetRide", new { id = ride.Id.ToString() }, ride);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while creating the ride: {ex.Message}" });
        }
    }


    [HttpGet("{id:length(24)}", Name = "GetRide")]
    public async Task<ActionResult<Ride>> GetRideById(string id)
    {
        try
        {
            var ride = await _rideService.GetRide(id);
            if (ride == null) return NotFound(new { message = "Ride not found" });
            return ride;
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while fetching the ride: {ex.Message}" });
        }
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateRide(string id, [FromBody] RideUpdateDTO updatedRideDTO)
    {
        try
        {
            var existingRide = await _rideService.GetRide(id);

            if (existingRide == null)
                return NotFound(new { message = "Ride not found, unable to update" });

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

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while updating the ride: {ex.Message}" });
        }
    }


    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteRide(string id)
    {
        try
        {
            var ride = await _rideService.GetRide(id);
            if (ride == null) return NotFound(new { message = "Ride not found, unable to delete" });
            await _rideService.DeleteRide(ride.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"An error occurred while deleting the ride: {ex.Message}" });
        }
    }
}
