using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using DataAccess.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<User,Role,int>, IAppDbContext
    {
        private IMongoClient _mongoClient { get; }
        private string _mongoDbName { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options, 
            IConfiguration configuration,
            string dbName = "SparkMongoDB")
            : base(options)
        {
            _mongoDbName = dbName;
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _mongoClient = client;
            MongoDb = client.GetDatabase(dbName);
        }
        public AppDbContext(DbContextOptions<AppDbContext> options, 
            IMongoClient client, 
            string dbName = "SparkMongoDB")
            : base(options)
        {
            _mongoDbName = dbName;
            _mongoClient = client;
            MongoDb = client.GetDatabase(dbName);
        }

        private CancellationToken _token = default;

        public ISimpleDbSet<Message> Messages =>
            new SimpleMongoDbSet<Message>(MongoDb.GetCollection<Message>("messages"), _token); 

        public ISimpleDbSet<Chat> Chats => 
            new SimpleMongoDbSet<Chat>(MongoDb.GetCollection<Chat>("chats"), _token);

        public ISimpleDbSet<PrivateChat> PrivateChats =>
            new SimpleMongoDbSet<PrivateChat>(MongoDb.GetCollection<Chat>("chats").OfType<PrivateChat>(), _token);

        public ISimpleDbSet<Channel> Channels =>
            new SimpleMongoDbSet<Channel>(MongoDb.GetCollection<Chat>("chats").OfType<Channel>(), _token);

        public ISimpleDbSet<Media> Media => 
            new SimpleMongoDbSet<Media>(MongoDb.GetCollection<Media>("media"), _token);

        public ISimpleDbSet<Server> Servers => 
            new SimpleMongoDbSet<Server>(MongoDb.GetCollection<Server>("servers"), _token);
        
        //public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;
        public IMongoDatabase MongoDb { get; }

        public void SetToken(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
        }

        public async Task CheckRemoveMedia(string id)
        {
            if(!ObjectId.TryParse(id, out var objectId)) return;
            
            long count = 0;
            count += await PrivateChats.CountAsync(c => c.Image != null && c.Image.Contains(id) );
            count += await Messages.CountAsync(m => m.Attachments.Any(a => a.Path.Contains(id)));
            count += await Servers.CountAsync(s => s.Image != null && s.Image.Contains(id));
            count += await Users.Where(u => u.Avatar != null && u.Avatar.Contains(id)).CountAsync(_token);
            
            if (count > 0) return;

            await Media.DeleteAsync(objectId);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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

        public async Task<List<Message>> GetMessagesAsync(
            string chatId,
            int skip,
            int take)
            => await MongoDb.GetCollection<Message>("messages")
                .Find(Builders<Message>.Filter.Eq("ChatId", chatId))
                .SortByDescending(m => m.SendTime)
                .Skip(skip)
                .Limit(take)
                .ToListAsync(_token);

        public async Task<List<Message>> GetPinnedMessagesAsync(
            string chatId
        ) => await MongoDb.GetCollection<Message>("messages")
            .Find(
                Builders<Message>.Filter.Eq("ChatId", chatId) &
                Builders<Message>.Filter.Eq("IsPinned", true)
            )
            .SortByDescending(m => m.PinnedTime)
            .ToListAsync(_token);
    }
}