using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public ISimpleDbSet<UserConnections, Guid> UserConnections =>
            new SimpleMongoDbSet<UserConnections, Guid>(MongoDb.GetCollection<UserConnections>("userConnections"), _token);
        public ISimpleDbSet<Message, string> Messages =>
            new SimpleMongoDbSet<Message, string>(MongoDb.GetCollection<Message>("messages"), _token);

        public ISimpleDbSet<Chat, string> Chats =>
            new SimpleMongoDbSet<Chat, string>(MongoDb.GetCollection<Chat>("chats"), _token);

        public ISimpleDbSet<PersonalChat, string> PersonalChats =>
            new SimpleMongoDbSet<PersonalChat, string>(MongoDb.GetCollection<Chat>("chats").OfType<PersonalChat>(), _token);
        public ISimpleDbSet<GroupChat, string> GroupChats =>
            new SimpleMongoDbSet<GroupChat, string>(MongoDb.GetCollection<Chat>("chats").OfType<GroupChat>(), _token);

        public ISimpleDbSet<Channel, string> Channels =>
            new SimpleMongoDbSet<Channel, string>(MongoDb.GetCollection<Chat>("chats").OfType<Channel>(), _token);

        public ISimpleDbSet<Media, string> Media =>
            new SimpleMongoDbSet<Media, string>(MongoDb.GetCollection<Media>("media"), _token);

        public ISimpleDbSet<Server, string> Servers =>
            new SimpleMongoDbSet<Server, string>(MongoDb.GetCollection<Server>("servers"), _token);

        public ISimpleDbSet<Invitation, string> Invitations =>
            new SimpleMongoDbSet<Invitation, string>(MongoDb.GetCollection<Invitation>("invitations"), _token);
        public ISimpleDbSet<RelationshipList, Guid> RelationshipLists =>
            new SimpleMongoDbSet<RelationshipList, Guid>(MongoDb.GetCollection<RelationshipList>("relationships"),
                _token);

        public ISimpleDbSet<Role, Guid> SqlRoles => new SimpleSqlDbSet<Role>(Roles, this, _token);
        public ISimpleDbSet<User, Guid> SqlUsers => new SimpleSqlDbSet<User>(Users, this, _token);
        public DbSet<UserProfile> UserProfiles { get; set; }
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

        public async Task<List<IdentityRoleClaim<Guid>>> GetRoleClaimAsync(Role role)
        {
            return await RoleClaims
                .Where(t => t.RoleId == role.Id)
                .ToListAsync();
        }

        public async Task AddClaimToRoleAsync(Role role, IdentityRoleClaim<Guid> claim)
        {
            await AddClaimsToRoleAsync(role, new List<IdentityRoleClaim<Guid>> { claim });
        }

        public async Task AddClaimsToRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims)
        {
            foreach (IdentityRoleClaim<Guid> claim in claims)
            {
                claim.RoleId = role.Id;
                await RoleClaims.AddAsync(claim);
            }
        }

        public async Task RemoveClaimsFromRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims)
        {
            for (int i = claims.Count() - 1; i >= 0; i--)
            {
                IdentityRoleClaim<Guid> claim = claims.ElementAt(i);

                await RemoveClaimFromRoleAsync(role, claim);
            }
        }

        public async Task RemoveClaimsFromRoleAsync(Role role)
        {
            List<IdentityRoleClaim<Guid>> claimToRemove = await RoleClaims
                .Where(rc => rc.RoleId == role.Id).ToListAsync();

            RoleClaims.RemoveRange(claimToRemove);
        }

        public async Task RemoveClaimFromRoleAsync(Role role, IdentityRoleClaim<Guid> claim)
        {
            RoleClaims.Remove(claim);
            await Task.CompletedTask;
        }
    }
}