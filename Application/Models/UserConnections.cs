using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

﻿namespace Application.Models
{
    public class UserConnections
    {
        [BsonId]
        public int UserId { get; set; }
        public HashSet<string> Connections { get; set; }
    }
}