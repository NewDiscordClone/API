using Application.Interfaces;
using Application.Mapping;
using Application.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using AutoMapper;

namespace Tests.Common
{
    public static class TestDbContextFactory
    {
        public static int UserAId { get; set; } = 1;
        public static int UserBId { get; set; } = 2;
        public static int UserCId { get; set; } = 3;
        public static int UserDId { get; set; } = 4;

        public static int ServerIdForDelete { get; set; } = 1;
        public static int ServerIdForUpdate { get; set; } = 2;

        public static ObjectId Channel1 { get; set; }
        public static ObjectId Channel2 { get; set; }

        public static ObjectId PrivateChat3 { get; set; }
        public static ObjectId PrivateChat4 { get; set; }
        public static ObjectId PrivateChat5 { get; set; }
        public static ObjectId PrivateChat6 { get; set; }
        public static ObjectId PrivateChat7 { get; set; } 
        
        public static ObjectId Message1 { get; set; } 
        public static ObjectId Message2 { get; set; } 

        private static IMongoClient _mongoClient;
        private const string _mongoDbName = "TestDatabase";

        public static IAppDbContext Create()
        {
            DbContextOptions<AppDbContext> options =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _mongoClient = new MongoClient("mongodb://localhost:27017");
            AppDbContext context = new(options, _mongoClient, _mongoDbName);
            context.Database.EnsureCreated();

            User userA = new()
            {
                Id = UserAId,
                UserName = "User A",
                //AvatarPath = null,
                Email = "email@test1.com",
            };
            User userB = new()
            {
                Id = UserBId,
                UserName = "User B",
                // AvatarPath = null,
                Email = "email@test2.com",
            };
            User userC = new()
            {
                Id = UserCId,
                UserName = "User C",
                // AvatarPath = null,
                Email = "email@test3.com",
            };
            User userD = new()
            {
                Id = UserDId,
                UserName = "User D",
                // AvatarPath = null,
                Email = "email@test4.com",
            };
            
            context.Users.AddRange(userA, userB, userC, userD);
            
            context.Servers.AddRange(
                new Server
                {
                    Id = ServerIdForDelete,
                    Title = "Server 1",
                    Owner = userA,
                    ServerProfiles =
                    {
                        new ServerProfile
                        {
                            User = userA
                        }
                    },
                    Roles = new List<Role>()
                },
                new Server
                {
                    Id = ServerIdForUpdate,
                    Title = "Server 2",
                    Owner = userB,
                    ServerProfiles =
                    {
                        new ServerProfile
                        {
                            User = userB
                        }
                    },
                    Roles = new List<Role>()
                });
            context.Channels.InsertMany(new List<Channel>()
            {
                new Channel
                {
                    Id = Channel1 = ObjectId.GenerateNewId(),
                    Title = "Channel 1",
                    ServerId = ServerIdForDelete
                },
                new Channel
                {
                    Id = Channel2 = ObjectId.GenerateNewId(),
                    Title = "Channel 2",
                    ServerId = ServerIdForUpdate
                }
            });

            IMapper mapper = new MapperConfiguration(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly))).CreateMapper();

            context.PrivateChats.InsertMany(new List<PrivateChat>()
                {
                    new()
                    {
                        Id = PrivateChat3 = ObjectId.GenerateNewId(),
                        OwnerId = userA.Id,
                        Users = { mapper.Map<UserLookUp>(userA), mapper.Map<UserLookUp>(userB) },
                    },
                    new()
                    {
                        Id = PrivateChat4 = ObjectId.GenerateNewId(),
                        OwnerId = userA.Id,
                        Users = { mapper.Map<UserLookUp>(userA), mapper.Map<UserLookUp>(userC) }
                    },
                    new()
                    {
                        Id = PrivateChat5 = ObjectId.GenerateNewId(),
                        OwnerId = userB.Id,
                        Users = { mapper.Map<UserLookUp>(userB), mapper.Map<UserLookUp>(userC) }
                    },
                    new()
                    {
                        Id = PrivateChat6 = ObjectId.GenerateNewId(),
                        OwnerId = userB.Id,
                        Users =
                        {
                            mapper.Map<UserLookUp>(userA),
                            mapper.Map<UserLookUp>(userB),
                            mapper.Map<UserLookUp>(userC),
                            mapper.Map<UserLookUp>(userD)
                        }
                    },
                    new()
                    {
                        Id = PrivateChat7 = ObjectId.GenerateNewId(),
                        OwnerId = userB.Id,
                        Users =
                        {
                            mapper.Map<UserLookUp>(userB), mapper.Map<UserLookUp>(userC), mapper.Map<UserLookUp>(userD)
                        }
                    }
                }
            );
            context.Messages.InsertMany(new List<Message>
            {
                new Message
                {
                    Id = Message1 = ObjectId.GenerateNewId(),
                    Text = "Message 1",
                    SendTime = DateTime.Now,
                    User = mapper.Map<UserLookUp>(userA),
                    ChatId = PrivateChat3,
                    Reactions =
                    {
                        new Reaction
                        {
                            Emoji = "☻",
                            User = mapper.Map<UserLookUp>(userB),
                        },
                        new Reaction
                        {
                            Emoji = "☺",
                            User = mapper.Map<UserLookUp>(userA),
                        }
                    }
                },
                new Message
                {
                    Id = Message2 = ObjectId.GenerateNewId(),
                    Text= "Message 2",
                    SendTime = DateTime.Now,
                    User = mapper.Map<UserLookUp>(userB),
                    IsPinned = true,
                    ChatId = PrivateChat3,
                    Attachments =
                    {
                        new Attachment
                        {
                            Type = AttachmentType.Url,
                            Path = "http://localhost:3000"
                        }
                    }
                }
            });

            context.SaveChanges();
            return context;
        }

        public static void Destroy(IAppDbContext iContext)
        {
            if(iContext is not AppDbContext context) return;
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}