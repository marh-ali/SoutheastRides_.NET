using SoutheastRides.Models;

public interface IRideRepository
{
    Task<IEnumerable<Ride>> GetAll();
    Task<Ride> Get(string id);
    Task<Ride> Create(Ride ride);
    Task Update(string id, Ride ride);
    Task Delete(string id);
}
