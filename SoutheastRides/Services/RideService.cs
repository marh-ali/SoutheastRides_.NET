public class RideService : IRideService
{
    private readonly IRideRepository _rideRepository;

    public RideService(IRideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    public async Task<IEnumerable<Ride>> GetAllRides()
    {
            var rides = await _rideRepository.GetAll();
            return rides ?? Enumerable.Empty<Ride>();
    }

    public async Task<Ride> GetRide(string id)
    {
        ValidateRideId(id);

        try
        {
            var ride = await _rideRepository.Get(id);
            if (ride == null)
                throw new Exception($"The ride with ID: {id} does not exist.");

            return ride;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while fetching the ride: {ex.Message}");
        }
    }

    public async Task<Ride> CreateRide(Ride ride)
    {
        if (ride == null)
            throw new ArgumentNullException(nameof(ride), "The provided ride data cannot be null.");
        try
        {
            return await _rideRepository.Create(ride);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the ride: {ex.Message}");
        }
    }

    public async Task UpdateRide(string id, Ride ride)
    {
        ValidateRideId(id);

        try
        {
            var existingRide = await _rideRepository.Get(id);
            if (existingRide == null)
                throw new Exception($"The ride with ID: {id} does not exist. Cannot perform update operation.");

            await _rideRepository.Update(id, ride);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the ride: {ex.Message}");
        }
    }

    public async Task DeleteRide(string id)
    {
        ValidateRideId(id);

        try
        {
            var ride = await _rideRepository.Get(id);
            if (ride == null)
                throw new Exception($"The ride with ID: {id} does not exist. Cannot perform delete operation.");

            await _rideRepository.Delete(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the ride: {ex.Message}");
        }
    }

    private void ValidateRideId(string id)
    {
        if (id.Length != 24)
            throw new ArgumentException("Ride ID must be exactly 24 hexadecimal characters.");
    }
}
