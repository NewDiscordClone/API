using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models
{
    public class Invitation
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
        /// <summary>
        /// Unique Id as an ObjectId representation of a user that make the invitation
        /// </summary>
        /// <example>5f95a3c3d0ddad0017ea9291</example>
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public int? UserId { get; set; }
        /// <summary>
        /// Unique Id as an ObjectId representation of a server to invite to
        /// </summary>
        /// <example>5f95a3c3d0ddad0017ea9291</example>
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; set; }
        
        public DateTime? ExpireTime { get; set; }
    }
}