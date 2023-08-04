using SoutheastRides.Models;

namespace Tests.Common
{
    public static class TestsHelper
    {
        private static List<Ride> _rides = new List<Ride>();

        private static List<Rsvp> _rsvps = new List<Rsvp>();

        private static List<User> _users = new List<User>();

        public static Ride CreateMockRide(string id = "1234567890", string creatorId = "creator123")
        {
            var ride = new Ride
            {
                Id = id,
                CreatorId = creatorId,
                Title = "Sample Ride",
                StartLocation = new string[] { "40.7128", "74.0060" },
                EndLocation = new string[] { "34.0522", "118.2437" },
                StartTime = DateTime.Now,
                Distance = 10000,
                Status = "Scheduled"
            };
            _rides.Add(ride);
            return ride;
        }

        public static Rsvp CreateMockRsvp(string rideId = "1234567890", string userId = "user123", string status = "Attending")
        {
            var rsvp = new Rsvp
            {
                RideId = rideId,
                UserId = userId,
                RsvpStatus = status
            };
            _rsvps.Add(rsvp);
            return rsvp;
        }

        public static User CreateMockUser(string id = "user123")
        {
            var user = new User
            {
                Id = id,
                Username = "SampleUser",
                AuthProvider = "Google",
                AuthProviderId = "google123"
            };
            _users.Add(user);
            return user;
        }

        public static void ClearMocks()
        {
            _rides.Clear();
            _rsvps.Clear();
            _users.Clear();
        }

    }
}
