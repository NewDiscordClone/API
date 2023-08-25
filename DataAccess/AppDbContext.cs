using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using DataAccess.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<User, Role, int>, IAppDbContext
    {
        private IMongoClient _mongoClient { get; }
        private string _mongoDbName { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration,
            string dbName = "SparkMongoDB")
            : base(options)
        {
            _mongoDbName = dbName;
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _mongoClient = client;
            MongoDb = client.GetDatabase(dbName);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options, IMongoClient client, string dbName = "SparkMongoDB")
            : base(options)
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

        public DbSet<Server> Servers { get; set; } = null!;
        public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;

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
            count += await Users.Where(u => u.Avatar != null && u.Avatar.Contains(id)).CountAsync(cancellationToken);
            count += await Servers.Where(s => s.Image != null && s.Image.Contains(id)).CountAsync(cancellationToken);
            
            if (count > 0) return;

            await Media.DeleteOneAsync(
                GetIdFilter<Media>(mediaId),
                null,
                cancellationToken);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new ServerConfiguration());
            builder.ApplyConfiguration(new ServerProfileConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(builder);
        }

        public override void Dispose()
        {
            _mongoClient.DropDatabase(_mongoDbName);
            base.Dispose();
        }

        public async Task<TEntity> FindSqlByIdAsync<TEntity>(int id, CancellationToken cancellationToken = default,
            params string[] includedProperties) where TEntity : class
        {
            DbSet<TEntity> dbSet = Set<TEntity>();
            IQueryable<TEntity> queryable = dbSet.AsQueryable();

            foreach (string property in includedProperties)
            {
                queryable = queryable.Include(property);
            }

            // Define an expression that represents the ID property
            Expression<Func<TEntity, bool>> predicate = entity =>
                EF.Property<int>(entity, "Id") == id;

            TEntity? entity = await queryable
                .FirstOrDefaultAsync(predicate, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found");
            }

            return entity;
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

            throw new InvalidOperationException($"Unhandled entity type: {type.Name}");
        }
    }
}