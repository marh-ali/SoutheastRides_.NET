using System.Collections.Generic;
using System.Threading.Tasks;
using SoutheastRides.Models;

namespace SoutheastRides.Services
{
    public interface IRsvpService
    {
        Task<IEnumerable<Rsvp>> GetAllRsvps();
        Task<Rsvp> GetRsvp(string id);
        Task<Rsvp> CreateRsvp(Rsvp rsvp);
        Task UpdateRsvp(string id, Rsvp rsvp);
        Task DeleteRsvp(string id);
    }
}
