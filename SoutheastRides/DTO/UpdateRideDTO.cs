using System;
namespace SoutheastRides.DTO
{
    public class RideUpdateDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string[]? StartLocation { get; set; }
        public string[]? EndLocation { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Status { get; set; }
    }

}

