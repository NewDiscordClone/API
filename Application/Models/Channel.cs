using MongoDB.Bson;

namespace Application.Models;

public class Channel : Chat
{
    public string Title { get; set; }

    public ObjectId ServerId { get; set; }
    
    
}