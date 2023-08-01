using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SoutheastRides.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Username")]
        [BsonRequired]
        public string Username { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("ProfilePictureUrl")]
        public string? ProfilePictureUrl { get; set; }

        [BsonElement("AuthProvider")]
        [BsonRequired]
        public string AuthProvider { get; set; }

        [BsonElement("AuthProviderId")]
        [BsonRequired]
        public string AuthProviderId { get; set; }

        [BsonElement("Bio")]
        public string? Bio { get; set; }

        [BsonElement("RideHistory")]
        public string[]? RideHistory { get; set; }

        [BsonElement("FavoriteRoutes")]
        public string[]? FavoriteRoutes { get; set; }
    }
}
