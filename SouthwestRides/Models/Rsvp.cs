using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BikeApp.Models
{
    public class Rsvp
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("RideId")]
        public string RideId { get; set; }

        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
