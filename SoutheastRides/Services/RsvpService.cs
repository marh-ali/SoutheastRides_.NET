using System.Collections.Generic;
using System.Threading.Tasks;
using SoutheastRides.Models;

namespace SoutheastRides.Services
{
    public class RsvpService : IRsvpService
    {
        private readonly IRsvpRepository _rsvpRepository;

        public RsvpService(IRsvpRepository rsvpRepository)
        {
            _rsvpRepository = rsvpRepository;
        }

        public async Task<IEnumerable<Rsvp>> GetAllRsvps()
        {
            return await _rsvpRepository.GetAll();
        }

        public async Task<Rsvp> GetRsvp(string id)
        {
            return await _rsvpRepository.Get(id);
        }

        public async Task<Rsvp> CreateRsvp(Rsvp rsvp)
        {
            return await _rsvpRepository.Create(rsvp);
        }

        public async Task UpdateRsvp(string id, Rsvp rsvp)
        {
            await _rsvpRepository.Update(id, rsvp);
        }

        public async Task DeleteRsvp(string id)
        {
            await _rsvpRepository.Delete(id);
        }
    }
}
