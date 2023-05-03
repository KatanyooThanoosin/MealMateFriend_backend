using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace main_backend.Models{
    public class PostModel{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Owner { get; set; } = null!;

        public decimal Limit { get; set; }

        public decimal Time { get; set; }

        public string Status { get; set; } = null!;

        public int ImgIndex { get; set; } = 0;

        [BsonElement("items")]
        [JsonPropertyName("items")]
        public List<int> ImgOrderIndexList{ get; set; } = null!;
        
    }
}