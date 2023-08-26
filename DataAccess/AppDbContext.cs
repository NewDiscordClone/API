using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess
{
    public class AppDbContext : IAppDbContext
    {
        private IMongoClient _mongoClient { get; }
        private string _mongoDbName { get; }

        public AppDbContext(IConfiguration configuration)
        {
            _mongoDbName = configuration["MongoDBName"]?? "SparkMongoDB";
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _mongoClient = client;
            MongoDb = client.GetDatabase(_mongoDbName);
        }

        public AppDbContext(IMongoClient client, string dbName)
        {
            _mongoDbName = dbName;
            _mongoClient = client;
            MongoDb = client.GetDatabase(dbName);
        }

        public IMongoCollection<Message> Messages => MongoDb.GetCollection<Message>("messages");

        public IMongoCollection<Chat> Chats => MongoDb.GetCollection<Chat>("chats");

        public IMongoCollection<PrivateChat> PrivateChats =>
            MongoDb.GetCollection<Chat>("chats").OfType<PrivateChat>();

        public IMongoCollection<Channel> Channels =>
            MongoDb.GetCollection<Chat>("chats").OfType<Channel>();

        public IMongoCollection<Media> Media => MongoDb.GetCollection<Media>("media");

        public IMongoCollection<Server> Servers => MongoDb.GetCollection<Server>("servers");
        public IMongoCollection<Role> Roles => MongoDb.GetCollection<Role>("roles");
        public IMongoCollection<User> Users => MongoDb.GetCollection<User>("users");
        //public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;

        public IMongoDatabase MongoDb { get; }

        public async Task CheckRemoveMedia(string id, CancellationToken cancellationToken = default)
        {
            if(!ObjectId.TryParse(id, out var mediaId)) return;

            var regex = new BsonRegularExpression(id, "i");
            long count = 0;
            count += await PrivateChats.CountDocumentsAsync(
                Builders<PrivateChat>.Filter.Regex(c => c.Image, regex),
                null,
                cancellationToken);
            count += await Messages.CountDocumentsAsync(
                Builders<Message>.Filter.ElemMatch(c => c.Attachments, 
                    Builders<Attachment>.Filter.Regex(c => c.Path, regex)),
                null,
                cancellationToken);
            count += await Servers.CountDocumentsAsync(
                Builders<Server>.Filter.Regex(s => s.Image, regex),
                null,
                cancellationToken);
            count += await Users.Find(u => u.Avatar != null && u.Avatar.Contains(id)).CountDocumentsAsync(cancellationToken);
            
            if (count > 0) return;

            await Media.DeleteOneAsync(
                GetIdFilter<Media>(mediaId),
                null,
                cancellationToken);

        }

        public void Dispose()
        {
            _mongoClient.DropDatabase(_mongoDbName);
        }

        public FilterDefinition<TEntity> GetIdFilter<TEntity>(ObjectId id)
        {
            return Builders<TEntity>.Filter.Eq("_id", id);
        }


        private async Task<TEntity> FindByIdAsync<TEntity>(IMongoCollection<TEntity> collection, ObjectId id,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            var filter = GetIdFilter<TEntity>(id);
            var result =
                await (await collection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefaultAsync(
                    cancellationToken);
            if (result == null) throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found");
            return result;
        }


        public async Task<TEntity> FindByIdAsync<TEntity>(ObjectId id, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var type = typeof(TEntity);
            if (type == typeof(Chat))
            {
                return (await FindByIdAsync(Chats, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }
            if (type == typeof(PrivateChat))
            {
                return (await FindByIdAsync(PrivateChats, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }
            if (type == typeof(Channel))
            {
                return (await FindByIdAsync(Channels, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }

            if (type == typeof(Message))
            {
                return (await FindByIdAsync(Messages, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }
            if (type == typeof(Media))
            {
                return (await FindByIdAsync(Media, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }
            if (type == typeof(Server))
            {
                return (await FindByIdAsync(Servers, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }
            if (type == typeof(User))
            {
                return (await FindByIdAsync(Users, id, cancellationToken) as TEntity) ??
                       throw new EntityNotFoundException($"{type.Name} {id} not found");
            }

            throw new InvalidOperationException($"Unhandled entity type: {type.Name}");
        }
    }
}