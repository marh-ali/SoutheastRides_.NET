public interface IRideService
{
    Task<IEnumerable<Ride>> GetAllRides();
    Task<Ride> GetRide(string id);
    Task<Ride> CreateRide(Ride ride);
    Task UpdateRide(string id, Ride ride);
    Task DeleteRide(string id);
}
