using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.DataAccess.Configurations;
using System.Security.Claims;

namespace Sparkle.DataAccess
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>, IAppDbContext
    {
        private IMongoClient _mongoClient { get; }
        private string _mongoDbName { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IConfiguration configuration,
            string dbName = "SparkMongoDB")
            : base(options)
        {
            _mongoDbName = dbName;
            MongoClient client = new(configuration.GetConnectionString("MongoDB"));
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

        public ISimpleDbSet<UserConnections> UserConnections =>
            new SimpleMongoDbSet<UserConnections, int>(MongoDb.GetCollection<UserConnections>("userConnections"), _token);
        public ISimpleDbSet<Message> Messages =>
            new SimpleMongoDbSet<Message>(MongoDb.GetCollection<Message>("messages"), _token);

        public ISimpleDbSet<Chat> Chats =>
            new SimpleMongoDbSet<Chat>(MongoDb.GetCollection<Chat>("chats"), _token);

        public ISimpleDbSet<PersonalChat> PersonalChats =>
            new SimpleMongoDbSet<PersonalChat>(MongoDb.GetCollection<Chat>("chats").OfType<PersonalChat>(), _token);
        public ISimpleDbSet<GroupChat> GroupChats =>
            new SimpleMongoDbSet<GroupChat>(MongoDb.GetCollection<Chat>("chats").OfType<GroupChat>(), _token);

        public ISimpleDbSet<Channel> Channels =>
            new SimpleMongoDbSet<Channel>(MongoDb.GetCollection<Chat>("chats").OfType<Channel>(), _token);

        public ISimpleDbSet<Media> Media =>
            new SimpleMongoDbSet<Media>(MongoDb.GetCollection<Media>("media"), _token);

        public ISimpleDbSet<Server> Servers =>
            new SimpleMongoDbSet<Server>(MongoDb.GetCollection<Server>("servers"), _token);

        public ISimpleDbSet<Invitation> Invitations =>
            new SimpleMongoDbSet<Invitation>(MongoDb.GetCollection<Invitation>("invitations"), _token);
        public ISimpleDbSet<RelationshipList> RelationshipLists =>
            new SimpleMongoDbSet<RelationshipList, Guid>(MongoDb.GetCollection<RelationshipList>("relationships"),
                _token);

        public ISimpleDbSet<Role> SqlRoles => new SimpleSqlDbSet<Role>(Roles, this, _token);
        public ISimpleDbSet<User> SqlUsers => new SimpleSqlDbSet<User>(Users, this, _token);

        //public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;
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

        public async Task<List<Claim>> GetRoleClaimAsync(Role role)
        {
            return await RoleClaims.Where(t => t.RoleId == role.Id)
                .Select(t => t.ToClaim()).ToListAsync();
        }

        public async Task AddClaimToRoleAsync(Role role, Claim claim)
        {
            await AddClaimsToRoleAsync(role, new List<Claim> { claim });
        }

        public async Task AddClaimsToRoleAsync(Role role, IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                await RoleClaims.AddAsync(new IdentityRoleClaim<Guid>
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                    RoleId = role.Id
                }, _token);
            }
            await SaveChangesAsync(_token);
        }
    }
}