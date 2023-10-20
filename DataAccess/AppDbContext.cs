using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>, IAppDbContext
    {
        private IMongoClient _mongoClient { get; }
        private string _mongoDbName { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IConfiguration configuration, ILogger<AppDbContext> logger)
            : base(options)
        {
            string connectionString = configuration.GetConnectionString("MongoDB")
                ?? throw new Exception("No mongo connection string provided");

            string connection = connectionString.Split(';')[0];
            string dbName = connectionString.Split(';')[1];

            logger.LogDebug($"Connection {connection} Db {dbName}");

            _mongoDbName = dbName;
            MongoClient client = new(connection);
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

        public IRepository<UserConnections, Guid> UserConnections =>
            new BaseMongoRepository<UserConnections, Guid>(MongoDb.GetCollection<UserConnections>("userConnections"), _token);
        public IRepository<Message, string> Messages =>
            new BaseMongoRepository<Message, string>(MongoDb.GetCollection<Message>("messages"), _token);

        public IRepository<Chat, string> Chats =>
            new BaseMongoRepository<Chat, string>(MongoDb.GetCollection<Chat>("chats"), _token);

        public IRepository<PersonalChat, string> PersonalChats =>
            new BaseMongoRepository<PersonalChat, string>(MongoDb.GetCollection<Chat>("chats").OfType<PersonalChat>(), _token);
        public IRepository<GroupChat, string> GroupChats =>
            new BaseMongoRepository<GroupChat, string>(MongoDb.GetCollection<Chat>("chats").OfType<GroupChat>(), _token);

        public IRepository<Channel, string> Channels =>
            new BaseMongoRepository<Channel, string>(MongoDb.GetCollection<Chat>("chats").OfType<Channel>(), _token);

        public IRepository<Media, string> Media =>
            new BaseMongoRepository<Media, string>(MongoDb.GetCollection<Media>("media"), _token);

        public IRepository<Server, string> Servers =>
            new BaseMongoRepository<Server, string>(MongoDb.GetCollection<Server>("servers"), _token);

        public IRepository<Invitation, string> Invitations =>
            new BaseMongoRepository<Invitation, string>(MongoDb.GetCollection<Invitation>("invitations"), _token);

        public IRepository<Role, Guid> SqlRoles => new BaseSqlRepository<Role, Guid>(this);
        public IRepository<User, Guid> SqlUsers => new BaseSqlRepository<User, Guid>(this);
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RoleUserProfile> RoleUserProfile { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        public IMongoDatabase MongoDb { get; }
        public void SetToken(CancellationToken cancellationToken)
        {
            _token = cancellationToken;
        }

        public async Task CheckRemoveMedia(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
                return;

            long count = 0;
            count += await GroupChats.CountAsync(c => c.Image != null && c.Image.Contains(id));
            count += await Messages.CountAsync(m => m.Attachments.Any(a => a.Path.Contains(id)));
            count += await Servers.CountAsync(s => s.Image != null && s.Image.Contains(id));
            count += await Users.Where(u => u.Avatar != null && u.Avatar.Contains(id)).CountAsync(_token);

            if (count > 0)
                return;

            await Media.DeleteAsync(id);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            builder.Entity<Role>()
                .Property(r => r.Id)
                .ValueGeneratedNever();

            builder.Entity<ServerProfile>().HasBaseType<UserProfile>();

            base.OnModelCreating(builder);
        }

        public override void Dispose()
        {
            _mongoClient.DropDatabase(_mongoDbName);
            base.Dispose();
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

        async Task IAppDbContext.SaveChangesAsync()
        {
            await SaveChangesAsync(_token);
        }
    }
}