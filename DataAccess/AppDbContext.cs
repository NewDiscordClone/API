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
        public IMongoDatabase MongoDb { get; }
        public ISimpleDbSet<UserProfile> UserProfiles => new SimpleSqlDbSet<UserProfile>(UserProfilesDbSet, this, _token);

        protected DbSet<UserProfile> UserProfilesDbSet { get; set; }
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