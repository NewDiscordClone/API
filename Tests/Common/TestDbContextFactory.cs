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
        private static IMongoClient _mongoClient;

        public static IAppDbContext Create(out Ids ids)
        {
            ids = new Ids();
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            AppDbContext context = new(_mongoClient, Guid.NewGuid().ToString());
            IMapper mapper = new MapperConfiguration(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly))).CreateMapper();

            User userA = new()
            {
                Id = ids.UserAId = ObjectId.GenerateNewId(),
                UserName = "User A",
                Avatar = null,
                Email = "email@test1.com",
            };
            User userB = new()
            {
                Id = ids.UserBId = ObjectId.GenerateNewId(),
                UserName = "User B",
                Avatar = null,
                Email = "email@test2.com",
            };
            User userC = new()
            {
                Id = ids.UserCId = ObjectId.GenerateNewId(),
                UserName = "User C",
                Avatar = null,
                Email = "email@test3.com",
            };
            User userD = new()
            {
                Id = ids.UserDId = ObjectId.GenerateNewId(),
                UserName = "User D",
                Avatar = null,
                Email = "email@test4.com",
            };
            User userFail = new()
            {
                Id = ids.UserFailId = ObjectId.GenerateNewId(),
                UserName = "User D",
                Avatar = null,
                Email = "email@test4.com",
            };

            context.Users.InsertMany(new List<User> { userA, userB, userC, userD, userFail });

            context.Servers.InsertMany(new List<Server>
            {
                new Server
                {
                    Id = ids.ServerIdForDelete = ObjectId.GenerateNewId(),
                    Title = "Server 1",
                    Owner = mapper.Map<UserLookUp>(userA),
                    ServerProfiles =
                    {
                        new ServerProfile
                        {
                            User = mapper.Map<UserLookUp>(userA)
                        }
                    },

                    // Roles = new List<Role>()
                },
                new Server
                {
                    Id = ids.ServerIdForUpdate = ObjectId.GenerateNewId(),
                    Title = "Server 2",
                    Owner = mapper.Map<UserLookUp>(userB),
                    ServerProfiles =
                    {
                        new ServerProfile
                        {
                            User = mapper.Map<UserLookUp>(userB)
                        }
                    },
                    // Roles = new List<Role>()
                }
            });
            context.Channels.InsertMany(new List<Channel>()
            {
                new Channel
                {
                    Id = ids.Channel1 = ObjectId.GenerateNewId(),
                    Title = "Channel 1",
                    ServerId = ids.ServerIdForDelete
                },
                new Channel
                {
                    Id = ids.Channel2 = ObjectId.GenerateNewId(),
                    Title = "Channel 2",
                    ServerId = ids.ServerIdForUpdate
                }
            });


            context.PrivateChats.InsertMany(new List<PrivateChat>()
                {
                    new()
                    {
                        Id = ids.PrivateChat3 = ObjectId.GenerateNewId(),
                        Title = "PrivateChat 3",
                        OwnerId = userA.Id,
                        Users = { mapper.Map<UserLookUp>(userA), mapper.Map<UserLookUp>(userB) },
                    },
                    new()
                    {
                        Id = ids.PrivateChat4 = ObjectId.GenerateNewId(),
                        Title = "PrivateChat 4",
                        OwnerId = userA.Id,
                        Users = { mapper.Map<UserLookUp>(userA), mapper.Map<UserLookUp>(userC) }
                    },
                    new()
                    {
                        Id = ids.PrivateChat5 = ObjectId.GenerateNewId(),
                        Title = "PrivateChat 5",
                        OwnerId = userB.Id,
                        Users = { mapper.Map<UserLookUp>(userB), mapper.Map<UserLookUp>(userC) }
                    },
                    new()
                    {
                        Id = ids.PrivateChat6 = ObjectId.GenerateNewId(),
                        Title = "PrivateChat 6",
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
                        Id = ids.PrivateChat7 = ObjectId.GenerateNewId(),
                        Title = "PrivateChat 7",
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
                    Id = ids.Message1 = ObjectId.GenerateNewId(),
                    Text = "Message 1",
                    SendTime = DateTime.Now,
                    User = mapper.Map<UserLookUp>(userA),
                    ChatId = ids.PrivateChat3,
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
                    Id = ids.Message2 = ObjectId.GenerateNewId(),
                    Text = "Message 2",
                    SendTime = DateTime.Now,
                    User = mapper.Map<UserLookUp>(userB),
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
            return context;
        }

        public static void Destroy(IAppDbContext iContext)
        {
            if (iContext is not AppDbContext context) return;
            context.Dispose();
        }
    }
}