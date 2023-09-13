using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models
{
    public class Media
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; set; }
        [DefaultValue("image.jpg")]
        public string FileName { get; set; }
        [DefaultValue("image/jpeg")]
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}