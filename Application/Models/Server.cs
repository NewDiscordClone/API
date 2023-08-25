using MongoDB.Driver;

namespace Application.Models;

public class Server
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }

    public virtual User Owner { get; set; }
    public virtual List<ServerProfile> ServerProfiles { get; set; } = new();
    public virtual List<Role> Roles { get; set; } = new();
    
    public async Task<List<Channel>> GetChannelsAsync(
        IMongoCollection<Channel> collection,
        CancellationToken cancellationToken = default
    ) => await collection
        .Find(Builders<Channel>.Filter.Eq("ServerId", Id))
        .ToListAsync(cancellationToken);
}