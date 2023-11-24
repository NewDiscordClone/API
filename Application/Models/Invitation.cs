using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models
{
    public record Invitation
    {
        /// <summary>
        /// Unique Id as an string representation of an ObjectId type
        /// </summary>
        /// <example>5f95a3c3d0ddad0017ea9291</example>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; set; }
        public Guid? UserId { get; set; }
        /// <summary>
        /// Unique Id as an ObjectId representation of a server to invite to
        /// </summary>
        /// <example>5f95a3c3d0ddad0017ea9291</example>
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; set; }

        public DateTime? ExpireTime { get; set; }

        public Invitation()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}