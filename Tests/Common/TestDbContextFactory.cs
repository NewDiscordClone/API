using Application.Interfaces;
using Application.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

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

        public static IAppDbContext Create()
        {
            DbContextOptions<AppDbContext> options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            AppDbContext context = new(options);
            context.Database.EnsureCreated();

            User userA = new()
            {
                Id = UserAId,
                UserName = "User A",
                AvatarPath = null,
                Email = "email@test1.com",
            };
            User userB = new()
            {
                Id = UserBId,
                UserName = "User B",
                AvatarPath = null,
                Email = "email@test2.com",
            };
            User userC = new()
            {
                Id = UserCId,
                UserName = "User C",
                AvatarPath = null,
                Email = "email@test3.com",
            };
            User userD = new()
            {
                Id = UserDId,
                UserName = "User D",
                AvatarPath = null,
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
                  Channels =
                    {
                            new Channel
                            {
                                Id = 1,
                                Title = "Channel 1"
                            }
                    }
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
                    Channels =
                    {
                            new Channel
                            {
                                Id = 2,
                                Title = "Channel 2"
                            }
                    }
                });

            context.PrivateChats.AddRange(
                new PrivateChat
                {
                    Id = 3,
                    OwnerId = userA.Id,
                    Users = { userA, userB },
                    Messages =
                    {
                        new Message
                        {
                            Id = 1,
                            Text= "Message 1",
                            SendTime = DateTime.Now,
                            User = userA,
                            Reactions =
                            {
                                new Reaction
                                {
                                    Id = 1,
                                    Emoji = "☻",
                                    User = userB,
                                },
                                new Reaction
                                {
                                    Id = 2,
                                    Emoji = "☺",
                                    User = userA,
                                }
                            }
                        },
                        new Message
                        {
                            Id = 2,
                            Text= "Message 2",
                            SendTime = DateTime.Now,
                            User = userB,
                            IsPinned = true,
                            Attachments =
                            {
                                new Attachment
                                {
                                    Id = 1,
                                    Type = AttachmentType.Url,
                                    Path = "http://localhost:3000"
                                }
                            }
                        }
                    }
                },
                new PrivateChat
                {
                    Id = 4,
                    OwnerId = userA.Id,
                    Users = { userA, userC }
                },
                new PrivateChat
                {
                    Id = 5,
                    OwnerId = userB.Id,
                    Users = { userB, userC }
                },
                new PrivateChat
                {
                    Id = 6,
                    OwnerId = userB.Id,
                    Users = { userA, userB, userC, userD }
                },
                new PrivateChat
                {
                    Id = 7,
                    OwnerId = userB.Id,
                    Users = { userB, userC, userD }
                }
            );

            context.SaveChanges();
            return context;
        }

        public static void Destroy(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }


}
