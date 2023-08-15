using System.Text;
using SoutheastRides.Models;

namespace Tests.Common
{
    public static class TestsHelper
    {
        private static List<Ride> _rides = new List<Ride>();

        private static List<Rsvp> _rsvps = new List<Rsvp>();

        private static List<User> _users = new List<User>();

        public static string GenerateRandomHexadecimalString(int length = 24)
        {
            if (length <= 0 || length % 2 != 0)
                throw new ArgumentException("Length must be a positive even number");

            var random = new Random();
            var bytes = new byte[length / 2]; // 2 hexadecimal characters represent 1 byte
            random.NextBytes(bytes);

            var stringBuilder = new StringBuilder(length);
            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("x2")); // Convert to hexadecimal
            }

            return stringBuilder.ToString();
        }

        public static Ride CreateMockRide()
        {
            var ride = new Ride
            {
                Id = GenerateRandomHexadecimalString(),
                CreatorId = GenerateRandomHexadecimalString(),
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

        public static Rsvp CreateMockRsvp()
        {
            var rsvp = new Rsvp
            {
                Id = GenerateRandomHexadecimalString(),
                RideId = GenerateRandomHexadecimalString(),
                UserId = GenerateRandomHexadecimalString(),
                RsvpStatus = "Accepted"
            };
            _rsvps.Add(rsvp);
            return rsvp;
        }

        public static User CreateMockUser()
        {
            var user = new User
            {
                Id = GenerateRandomHexadecimalString(),
                Username = "SampleUser",
                AuthProvider = "Google",
                AuthProviderId = "google123"
            };
            _users.Add(user);
            return user;
        }

        public static List<Ride> CreateMockRides(int count = 3)
        {
            _rides.Clear();
            for (int i = 0; i < count; i++)
            {
                CreateMockRide();
            }
            return _rides;
        }

        public static List<Rsvp> CreateMockRsvps(int count =3)
        {
            _rsvps.Clear();
            for (int i = 0; i < count; i++)
            {
                CreateMockRsvp();
            }
            return _rsvps;
        }

        public static List<User> CreateMockUsers(int count = 3)
        {
            _users.Clear();
            for (int i = 0; i < count; i++)
            {
                CreateMockUser();
            }
            return _users;
        }

        public static void ClearMocks()
        {
            _rides.Clear();
            _rsvps.Clear();
            _users.Clear();
        }

    }
}
