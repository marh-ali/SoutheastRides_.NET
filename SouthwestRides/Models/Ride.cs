using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BikeApp.Models
{
    public class Ride
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("StartLocation")]
        public string StartLocation { get; set; }

        [BsonElement("EndLocation")]
        public string EndLocation { get; set; }

        [BsonElement("StartTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("Creator")]
        public string Creator { get; set; }

        [BsonElement("Map")]
        public string Map { get; set; }
    }
}
