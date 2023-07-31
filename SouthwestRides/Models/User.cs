using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SouthwestRides.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("ProfilePictureUrl")]
        public string ProfilePictureUrl { get; set; }

        // We're keeping these properties to identify the user based on Firebase Auth info
        [BsonElement("AuthProvider")]
        public string AuthProvider { get; set; }

        [BsonElement("AuthProviderId")]
        public string AuthProviderId { get; set; }
    }
}
