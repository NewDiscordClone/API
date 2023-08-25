using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Application.Models;

public class Server
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }

    public UserLookUp Owner { get; set; }
    public List<ServerProfile> ServerProfiles { get; set; } = new();
    
    public async Task<List<Channel>> GetChannelsAsync(
        IMongoCollection<Channel> collection,
        CancellationToken cancellationToken = default
    ) => await collection
        .Find(Builders<Channel>.Filter.Eq(c => c.ServerId, Id))
        .ToListAsync(cancellationToken);
    
    public async Task<List<Role>> GetRolesAsync(
        IMongoCollection<Role> collection,
        CancellationToken cancellationToken = default
        ) => await collection
        .Find(Builders<Role>.Filter.Eq(r => r.ServerId, Id))
        .ToListAsync(cancellationToken);
}