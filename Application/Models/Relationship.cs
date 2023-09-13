﻿using MongoDB.Bson.Serialization.Attributes;

namespace Sparkle.Application.Models
{
    public enum RelationshipType
    {
        Acquaintance,
        Friend,
        Pending,
        Waiting,
        Blocked
    }
    public class RelationshipList
    {
        [BsonId]
        public Guid Id { get; set; }
        public List<Relationship> Relationships { get; set; }
    }
    public class Relationship
    {
        public Guid UserId { get; set; }
        public RelationshipType RelationshipType { get; set; }
    }
}