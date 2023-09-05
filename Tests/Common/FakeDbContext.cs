using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using DataAccess.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Security.Claims;
using DataAccess;

namespace Tests.Common
{
    public class FakeDbContext : IdentityDbContext<User, Role, Guid>, IAppDbContext
    {
        private readonly IMapper _mapper;

        public FakeDbContext(DbContextOptions<FakeDbContext> options, IMapper mapper)
            : base(options)
        {
            _mapper = mapper;
        }

        public void Create(HardCodedData hardCodedData)
        {
            Database.EnsureCreated();
            
            Users.AddRange(hardCodedData.Users);
            Servers = new SimpleFakeDbSet<Server>(hardCodedData.Servers);
            _chats = new SimpleFakeDbSet<Chat>(hardCodedData.GroupChats.Cast<Chat>().Concat(hardCodedData.Channels).ToList());
            Messages = new SimpleFakeDbSet<Message>(hardCodedData.Messages);
            Invitations = new SimpleFakeDbSet<Invitation>(hardCodedData.Invitations);
            SaveChanges();
        }

        private CancellationToken _token = default;

        public ISimpleDbSet<UserConnections> UserConnections => null!;
        public ISimpleDbSet<Message> Messages { get; set; }

        private SimpleFakeDbSet<Chat> _chats;

        public ISimpleDbSet<Chat> Chats => _chats;

        public ISimpleDbSet<PersonalChat> PersonalChats => new SimpleDerivedFakeDbSet<Chat, PersonalChat>(_chats);
        public ISimpleDbSet<GroupChat> GroupChats => new SimpleDerivedFakeDbSet<Chat, GroupChat>(_chats);

        public ISimpleDbSet<Channel> Channels => new SimpleDerivedFakeDbSet<Chat, Channel>(_chats);

        public ISimpleDbSet<Media> Media { get; set; } = new SimpleFakeDbSet<Media>(new List<Media>());

        public ISimpleDbSet<Server> Servers { get; set; }
        public ISimpleDbSet<Invitation> Invitations { get; set; } = new SimpleFakeDbSet<Invitation>(new List<Invitation>());
        public ISimpleDbSet<RelationshipList> RelationshipLists { get; set; } = new SimpleFakeDbSet<RelationshipList>(new List<RelationshipList>());
        public ISimpleDbSet<Role> SqlRoles => new SimpleSqlDbSet<Role>(Roles, this, _token);
        public ISimpleDbSet<User> SqlUsers => new SimpleSqlDbSet<User>(Users, this, _token);

        //public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;

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

            return entity
                ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found", id.ToString());
        }

        public async Task<List<Message>> GetMessagesAsync(string chatId, int skip, int take)
        {
            List<Message> list = await Messages.FilterAsync(m => m.ChatId == chatId);
            list.Sort((m1, m2) => m1.SendTime.Millisecond - m2.SendTime.Millisecond);
            return list.Skip(skip).Take(take).ToList();
        }

        public async Task<List<Message>> GetPinnedMessagesAsync(string chatId)
        {
            List<Message> list = await Messages.FilterAsync(m => m.ChatId == chatId && m.IsPinned);
            list.Sort((m1, m2) => m1.PinnedTime.Value.Millisecond - m2.PinnedTime.Value.Millisecond);
            return list;
        }
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