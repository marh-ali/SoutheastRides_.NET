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
            var rsvps = await _rsvpRepository.GetAll();
            return rsvps ?? Enumerable.Empty<Rsvp>();
        }


        public async Task<Rsvp> GetRsvp(string id)
        {
            ValidateRsvpId(id);

            try
            {
                var rsvp = await _rsvpRepository.Get(id);
                if (rsvp == null)
                    throw new Exception($"The RSVP with ID: {id} does not exist.");

                return rsvp;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching the RSVP: {ex.Message}");
            }
        }

        public async Task<Rsvp> CreateRsvp(Rsvp rsvp)
        {
            if (rsvp == null)
                throw new ArgumentNullException(nameof(rsvp), "The provided RSVP data cannot be null.");

            ValidateRsvpDetails(rsvp);

            try
            {
                return await _rsvpRepository.Create(rsvp);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the RSVP: {ex.Message}");
            }
        }

        public async Task UpdateRsvp(string id, Rsvp rsvp)
        {
            ValidateRsvpId(id);
            ValidateRsvpDetails(rsvp);

            try
            {
                var existingRsvp = await _rsvpRepository.Get(id);
                if (existingRsvp == null)
                    throw new Exception($"The RSVP with ID: {id} does not exist. Cannot perform update operation.");

                await _rsvpRepository.Update(id, rsvp);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the RSVP: {ex.Message}");
            }
        }

        public async Task DeleteRsvp(string id)
        {
            ValidateRsvpId(id);

            try
            {
                var rsvp = await _rsvpRepository.Get(id);
                if (rsvp == null)
                    throw new Exception($"The RSVP with ID: {id} does not exist. Cannot perform delete operation.");

                await _rsvpRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the RSVP: {ex.Message}");
            }
        }

        private void ValidateRsvpId(string id)
        {
            if (id.Length != 24)
                throw new ArgumentException("RSVP ID must be exactly 24 hexadecimal characters.");
        }

        private void ValidateRsvpDetails(Rsvp rsvp)
        {
            if (string.IsNullOrEmpty(rsvp.RideId) || string.IsNullOrEmpty(rsvp.UserId))
                throw new ArgumentException("RSVP details are incomplete. Please ensure all required fields are filled and that RideId & UserId are valid.");
        }
    }
}
