using MongoDB.Bson;

namespace Application.Models;

public class Channel : Chat
{
    public string Title { get; set; }

    public int ServerId { get; set; } //TODO: Поміняти на ObjectId якщо сервер також буде в mongodb
    
    
}