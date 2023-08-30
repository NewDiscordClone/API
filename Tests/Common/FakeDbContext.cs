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

namespace Tests.Common
{
    public class FakeDbContext : IdentityDbContext<User, Role, int>, IAppDbContext
    {
        private readonly IMapper _mapper;

        public FakeDbContext(DbContextOptions<FakeDbContext> options, IMapper mapper)
            : base(options)
        {
            _mapper = mapper;
        }

        public void Create(Ids ids)
        {
            Database.EnsureCreated();
            User userA = new()
            {
                Id = ids.UserAId,
                UserName = "User A",
                Avatar = null,
                Email = "email@test1.com",
            };
            User userB = new()
            {
                Id = ids.UserBId,
                UserName = "User B",
                Avatar = null,
                Email = "email@test2.com",
            };
            User userC = new()
            {
                Id = ids.UserCId,
                UserName = "User C",
                Avatar = null,
                Email = "email@test3.com",
            };
            User userD = new()
            {
                Id = ids.UserDId,
                UserName = "User D",
                Avatar = null,
                Email = "email@test4.com",
            };
            Users.AddRange(userA, userB, userC, userD);

            Servers = new SimpleFakeDbSet<Server>(new List<Server>
            {
                new Server
                {
                    Id = ids.ServerIdForDelete = ObjectId.GenerateNewId().ToString(),
                    Title = "Server 1",
                    Owner = _mapper.Map<UserLookUp>(userA),
                    ServerProfiles =
                    {
                        new ServerProfile
                        {
                            User = _mapper.Map<UserLookUp>(userA)
                        }
                    },

                    // Roles = new List<Role>()
                },
                new Server
                {
                    Id = ids.ServerIdForUpdate = ObjectId.GenerateNewId().ToString(),
                    Title = "Server 2",
                    Owner = _mapper.Map<UserLookUp>(userB),
                    ServerProfiles =
                    {
                        new ServerProfile
                        {
                            User = _mapper.Map<UserLookUp>(userB)
                        }
                    },
                    // Roles = new List<Role>()
                }
            });
            _chats = new SimpleTwoFakeDbSets<Chat, PrivateChat, Channel>(
            new List<PrivateChat>()
            {
                new()
                {
                    Id = ids.PrivateChat3 = ObjectId.GenerateNewId().ToString(),
                    Title = "PrivateChat 3",
                    OwnerId = userA.Id,
                    Users = { _mapper.Map<UserLookUp>(userA), _mapper.Map<UserLookUp>(userB) },
                },
                new()
                {
                    Id = ids.PrivateChat4 = ObjectId.GenerateNewId().ToString(),
                    Title = "PrivateChat 4",
                    OwnerId = userA.Id,
                    Users = { _mapper.Map<UserLookUp>(userA), _mapper.Map<UserLookUp>(userC) }
                },
                new()
                {
                    Id = ids.PrivateChat5 = ObjectId.GenerateNewId().ToString(),
                    Title = "PrivateChat 5",
                    OwnerId = userB.Id,
                    Users = { _mapper.Map<UserLookUp>(userB), _mapper.Map<UserLookUp>(userC) }
                },
                new()
                {
                    Id = ids.PrivateChat6 = ObjectId.GenerateNewId().ToString(),
                    Title = "PrivateChat 6",
                    OwnerId = userB.Id,
                    Users =
                    {
                        _mapper.Map<UserLookUp>(userA),
                        _mapper.Map<UserLookUp>(userB),
                        _mapper.Map<UserLookUp>(userC),
                        _mapper.Map<UserLookUp>(userD)
                    }
                },
                new()
                {
                    Id = ids.PrivateChat7 = ObjectId.GenerateNewId().ToString(),
                    Title = "PrivateChat 7",
                    OwnerId = userB.Id,
                    Users =
                    {
                        _mapper.Map<UserLookUp>(userB), _mapper.Map<UserLookUp>(userC), _mapper.Map<UserLookUp>(userD)
                    }
                }
            }, new List<Channel>()
            {
                new Channel
                {
                    Id = ids.Channel1 = ObjectId.GenerateNewId().ToString(),
                    Title = "Channel 1",
                    ServerId = ids.ServerIdForDelete
                },
                new Channel
                {
                    Id = ids.Channel2 = ObjectId.GenerateNewId().ToString(),
                    Title = "Channel 2",
                    ServerId = ids.ServerIdForUpdate
                }
            });
            Messages = new SimpleFakeDbSet<Message>(new List<Message>
            {
                new Message
                {
                    Id = ids.Message1 = ObjectId.GenerateNewId().ToString(),
                    Text = "Message 1",
                    SendTime = DateTime.Now,
                    User = _mapper.Map<UserLookUp>(userA),
                    ChatId = ids.PrivateChat3,
                    Reactions =
                    {
                        new Reaction
                        {
                            Emoji = "☻",
                            User = _mapper.Map<UserLookUp>(userB),
                        },
                        new Reaction
                        {
                            Emoji = "☺",
                            User = _mapper.Map<UserLookUp>(userA),
                        }
                    }
                },
                new Message
                {
                    Id = ids.Message2 = ObjectId.GenerateNewId().ToString(),
                    Text = "Message 2",
                    SendTime = DateTime.Now,
                    User = _mapper.Map<UserLookUp>(userB),
                    IsPinned = true,
                    ChatId = ids.PrivateChat3,
                    Attachments =
                    {
                        new Attachment
                        {
                            IsInText = false,
                            Path = "http://localhost:3000"
                        }
                    }
                }
            });
            SaveChanges();
        }

        private CancellationToken _token = default;

        public ISimpleDbSet<Message> Messages { get; set; }

        private SimpleTwoFakeDbSets<Chat, PrivateChat, Channel> _chats;

        public ISimpleDbSet<Chat> Chats => _chats;

        public ISimpleDbSet<PrivateChat> PrivateChats => _chats.DbSet1;

        public ISimpleDbSet<Channel> Channels => _chats.DbSet2;

        public ISimpleDbSet<Media> Media { get; set; } = new SimpleFakeDbSet<Media>(new List<Media>());

        public ISimpleDbSet<Server> Servers { get; set; }

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
            count += await PrivateChats.CountAsync(c => c.Image != null && c.Image.Contains(id));
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

            if (entity == null)
            {
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found");
            }

            return entity;
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
                await RoleClaims.AddAsync(new IdentityRoleClaim<int>
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                    RoleId = role.Id
                });
            }
            await SaveChangesAsync();
        }
    }
}