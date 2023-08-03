using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

public class Ride
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("CreatorId")]
    [Required]
    public string CreatorId { get; set; }

    [BsonElement("Title")]
    [Required]
    public string Title { get; set; }

    [BsonElement("Description")]
    public string? Description { get; set; } // Additional details about the ride

    [BsonElement("StartLocation")]
    [BsonRequired]
    public string[] StartLocation { get; set; } // Start location coordinates

    [BsonElement("EndLocation")]
    public string[] EndLocation { get; set; } // End location coordinates

    [BsonElement("StartTime")]
    [Required]
    public DateTime StartTime { get; set; } // Start time of the ride

    [BsonElement("EndTime")]
    public DateTime? EndTime { get; set; } // Optional end time of the ride

    [BsonElement("Distance")]
    public int Distance { get; set; } // Distance in meters

    [BsonElement("MaxParticipants")]
    public int? MaxParticipants { get; set; } // Optional limit on participants

    [BsonElement("Status")]
    [Required]
    public string Status { get; set; } // Status of the ride (Scheduled, Ongoing, Completed, Cancelled)
}
