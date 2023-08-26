using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Application.Models;

public class Role : MongoRole<ObjectId>
{
    public string Color { get; set; }
    public ObjectId ServerId { get; set; }
}