using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Rsvp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("RideId")]
    [BsonRequired]
    public string RideId { get; set; }

    [BsonElement("UserId")]
    [BsonRequired]
    public string UserId { get; set; }

    [BsonElement("RsvpStatus")]
    public string RsvpStatus { get; set; } // RSVP status (e.g., Accepted, Declined, Tentative)

    [BsonElement("CyclingExperience")]
    public string? CyclingExperience { get; set; } // Optional cycling experience level (e.g., Beginner, Intermediate, Expert)

    [BsonElement("Comment")]
    public string? Comment { get; set; } // Optional comments or special requirements
}
