using Application.Exceptions;
using Application.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Application.Models
{
    public class Media
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string Extension { get; set; }
    }
}