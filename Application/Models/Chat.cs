using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Application.Models;

public abstract class Chat
{
    [BsonId]
    public ObjectId Id { get; set; }
    public async Task<List<Message>> GetPinnedMessagesAsync(
        IMongoCollection<Message> collection,
        CancellationToken cancellationToken = default
    ) => await collection
        .Find(
            Builders<Message>.Filter.Eq("ChatId", Id) &
            Builders<Message>.Filter.Eq("IsPinned", true)
        )
        .ToListAsync(cancellationToken);

    public async Task<List<Message>> GetMessagesAsync(
        IMongoCollection<Message> collection,
        int skip,
        int take,
        CancellationToken cancellationToken = default
    ) => await collection
        .Find(Builders<Message>.Filter.Eq("ChatId", Id))
        .SortByDescending(m => m.SendTime)
        .Skip(skip)
        .Limit(take).ToListAsync(cancellationToken);

    public List<UserLookUp> Users { get; set; } = new();
}