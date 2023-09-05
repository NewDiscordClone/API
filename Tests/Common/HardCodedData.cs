using Application.Models;
using AutoMapper;
using MongoDB.Bson;

namespace Tests.Common
{
    public class HardCodedData
    {
        private Ids _ids;
        private IMapper _mapper;

        public HardCodedData(Ids ids, IMapper mapper)
        {
            _ids = ids;
            _mapper = mapper;
            _userA = new User
            {
                Id = _ids.UserAId = Guid.NewGuid(),
                UserName = "User A",
                Avatar = null,
                Email = "email@test1.com",
            };
            _userB = new User
            {
                Id = _ids.UserBId = Guid.NewGuid(),
                UserName = "User B",
                Avatar = null,
                Email = "email@test2.com",
            };
            _userC = new User
            {
                Id = _ids.UserCId = Guid.NewGuid(),
                UserName = "User C",
                Avatar = null,
                Email = "email@test3.com",
            };
            _userD = new User
            {
                Id = _ids.UserDId = Guid.NewGuid(),
                UserName = "User D",
                Avatar = null,
                Email = "email@test4.com",
            };
        }

        private readonly User _userA;
        private readonly User _userB;
        private readonly User _userC;
        private readonly User _userD;

        public List<User> Users => new List<User>()
        {
            _userA,
            _userB,
            _userC,
            _userD
        };

        public List<Role> Roles => new List<Role>()
        {
            new()
            {
                Name = "Owner",
                Color = "#FFFF00"
            }
        };

        public List<Server> Servers => new List<Server>
        {
            new Server
            {
                Id = _ids.Server1 = ObjectId.GenerateNewId().ToString(),
                Title = "Server 1",
                Owner = _ids.UserAId,
                ServerProfiles =
                {
                    new ServerProfile
                    {
                        UserId = _ids.UserAId
                    }
                },

                // Roles = new List<Role>()
            },
            new Server
            {
                Id = _ids.Server2 = ObjectId.GenerateNewId().ToString(),
                Title = "Server 2",
                Owner = _ids.UserBId,
                ServerProfiles =
                {
                    new ServerProfile
                    {
                        UserId = _ids.UserBId
                    }
                },
                // Roles = new List<Role>()
            },
            new Server
            {
                Id = _ids.Server3 = ObjectId.GenerateNewId().ToString(),
                Title = "Server 3",
                Owner = _ids.UserCId,
                ServerProfiles =
                {
                    new ServerProfile
                    {
                        UserId = _ids.UserAId
                    },
                    new ServerProfile
                    {
                        UserId = _ids.UserBId
                    },
                    new ServerProfile
                    {
                        UserId = _ids.UserCId
                    }
                },
                // Roles = new List<Role>()
            }
        };

        public List<Channel> Channels => new List<Channel>()
        {
            new Channel
            {
                Id = _ids.Channel1 = ObjectId.GenerateNewId().ToString(),
                Title = "Channel 1",
                ServerId = _ids.Server1,
                Users = new List<Guid>
                {
                    _ids.UserAId,
                }
            },
            new Channel
            {
                Id = _ids.Channel2 = ObjectId.GenerateNewId().ToString(),
                Title = "Channel 2",
                ServerId = _ids.Server2,
                Users = new List<Guid>
                {
                    _ids.UserBId,
                }
            },
            new Channel
            {
                Id = _ids.Channel3 = ObjectId.GenerateNewId().ToString(),
                Title = "Channel 3",
                ServerId = _ids.Server3,
                Users = new List<Guid>
                {
                    _ids.UserAId,
                    _ids.UserBId,
                    _ids.UserCId,
                }
            }
        };

        public List<GroupChat> GroupChats => new List<GroupChat>()
        {
            new GroupChat()
            {
                Id = _ids.GroupChat3 = ObjectId.GenerateNewId().ToString(),
                Title = "PersonalChat 3",
                OwnerId = _userA.Id,
                Users = { _ids.UserAId, _ids.UserBId, },
            },
            new GroupChat()
            {
                Id = _ids.GroupChat4 = ObjectId.GenerateNewId().ToString(),
                Title = "PersonalChat 4",
                OwnerId = _userA.Id,
                Users = { _ids.UserAId }
            },
            new GroupChat()
            {
                Id = _ids.GroupChat5 = ObjectId.GenerateNewId().ToString(),
                Title = "PersonalChat 5",
                OwnerId = _userB.Id,
                Users = { _ids.UserBId, _ids.UserCId, }
            },
            new GroupChat()
            {
                Id = _ids.GroupChat6 = ObjectId.GenerateNewId().ToString(),
                Title = "PersonalChat 6",
                OwnerId = _userB.Id,
                Users =
                {
                    _ids.UserAId,
                    _ids.UserBId,
                    _ids.UserCId,
                    _ids.UserDId
                }
            },
            new GroupChat()
            {
                Id = _ids.GroupChat7 = ObjectId.GenerateNewId().ToString(),
                Title = "PersonalChat 7",
                OwnerId = _userB.Id,
                Users =
                {
                    _ids.UserBId, _ids.UserCId, _ids.UserDId
                }
            }
        };

        public List<Message> Messages => new List<Message>
        {
            new Message
            {
                Id = _ids.Message1 = ObjectId.GenerateNewId().ToString(),
                Text = "Message 1",
                SendTime = DateTime.Now,
                User = _ids.UserAId,
                ChatId = _ids.GroupChat3,
                Reactions =
                {
                    new Reaction
                    {
                        Emoji = "☻",
                        User = _ids.UserBId,
                    },
                    new Reaction
                    {
                        Emoji = "☺",
                        User = _ids.UserAId,
                    }
                }
            },
            new Message
            {
                Id = _ids.Message2 = ObjectId.GenerateNewId().ToString(),
                Text = "Message 2",
                SendTime = DateTime.Now,
                User = _ids.UserBId,
                IsPinned = true,
                ChatId = _ids.GroupChat3,
                Attachments =
                {
                    new Attachment
                    {
                        IsInText = false,
                        Path = "http://localhost:3000"
                    }
                }
            }
        };
    }
}