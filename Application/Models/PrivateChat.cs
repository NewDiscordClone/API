using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;

namespace Application.Models;

public class PrivateChat : Chat
{
    public string? Image { get; set; }
    public string? Title { get; set; }
    public ObjectId OwnerId { get; set; }
}