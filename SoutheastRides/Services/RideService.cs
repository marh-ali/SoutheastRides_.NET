
public class RideService : IRideService
{
    private readonly IRideRepository _rideRepository;

    public RideService(IRideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    public async Task<IEnumerable<Ride>> GetAllRides()
    {
        return await _rideRepository.GetAll();
    }

    public async Task<Ride> GetRide(string id)
    {
        return await _rideRepository.Get(id);
    }

    public async Task<Ride> CreateRide(Ride ride)
    {
        return await _rideRepository.Create(ride);
    }

    public async Task UpdateRide(string id, Ride ride)
    {
        await _rideRepository.Update(id, ride);
    }

    public async Task DeleteRide(string id)
    {
        await _rideRepository.Delete(id);
    }
}
