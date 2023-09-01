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
                Id = _ids.UserAId,
                UserName = "User A",
                Avatar = null,
                Email = "email@test1.com",
            };
            _userB = new User
            {
                Id = _ids.UserBId,
                UserName = "User B",
                Avatar = null,
                Email = "email@test2.com",
            };
            _userC = new User
            {
                Id = _ids.UserCId,
                UserName = "User C",
                Avatar = null,
                Email = "email@test3.com",
            };
            _userD = new User
            {
                Id = _ids.UserDId,
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
                Id = _ids.ServerIdForDelete = ObjectId.GenerateNewId().ToString(),
                Title = "Server 1",
                Owner = _mapper.Map<UserLookUp>(_userA),
                ServerProfiles =
                {
                    new ServerProfile
                    {
                        User = _mapper.Map<UserLookUp>(_userA)
                    }
                },

                // Roles = new List<Role>()
            },
            new Server
            {
                Id = _ids.ServerIdForUpdate = ObjectId.GenerateNewId().ToString(),
                Title = "Server 2",
                Owner = _mapper.Map<UserLookUp>(_userB),
                ServerProfiles =
                {
                    new ServerProfile
                    {
                        User = _mapper.Map<UserLookUp>(_userB)
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
                ServerId = _ids.ServerIdForDelete
            },
            new Channel
            {
                Id = _ids.Channel2 = ObjectId.GenerateNewId().ToString(),
                Title = "Channel 2",
                ServerId = _ids.ServerIdForUpdate
            }
        };

        public List<PrivateChat> PrivateChats => new List<PrivateChat>()
        {
            new GroupChat()
            {
                Id = _ids.PrivateChat3 = ObjectId.GenerateNewId().ToString(),
                Title = "PrivateChat 3",
                OwnerId = _userA.Id,
                Users = { _mapper.Map<UserLookUp>(_userA), _mapper.Map<UserLookUp>(_userB) },
            },
            new GroupChat()
            {
                Id = _ids.PrivateChat4 = ObjectId.GenerateNewId().ToString(),
                Title = "PrivateChat 4",
                OwnerId = _userA.Id,
                Users = { _mapper.Map<UserLookUp>(_userA) }
            },
            new GroupChat()
            {
                Id = _ids.PrivateChat5 = ObjectId.GenerateNewId().ToString(),
                Title = "PrivateChat 5",
                OwnerId = _userB.Id,
                Users = { _mapper.Map<UserLookUp>(_userB), _mapper.Map<UserLookUp>(_userC) }
            },
            new GroupChat()
            {
                Id = _ids.PrivateChat6 = ObjectId.GenerateNewId().ToString(),
                Title = "PrivateChat 6",
                OwnerId = _userB.Id,
                Users =
                {
                    _mapper.Map<UserLookUp>(_userA),
                    _mapper.Map<UserLookUp>(_userB),
                    _mapper.Map<UserLookUp>(_userC),
                    _mapper.Map<UserLookUp>(_userD)
                }
            },
            new GroupChat()
            {
                Id = _ids.PrivateChat7 = ObjectId.GenerateNewId().ToString(),
                Title = "PrivateChat 7",
                OwnerId = _userB.Id,
                Users =
                {
                    _mapper.Map<UserLookUp>(_userB), _mapper.Map<UserLookUp>(_userC), _mapper.Map<UserLookUp>(_userD)
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
                User = _mapper.Map<UserLookUp>(_userA),
                ChatId = _ids.PrivateChat3,
                Reactions =
                {
                    new Reaction
                    {
                        Emoji = "☻",
                        User = _mapper.Map<UserLookUp>(_userB),
                    },
                    new Reaction
                    {
                        Emoji = "☺",
                        User = _mapper.Map<UserLookUp>(_userA),
                    }
                }
            },
            new Message
            {
                Id = _ids.Message2 = ObjectId.GenerateNewId().ToString(),
                Text = "Message 2",
                SendTime = DateTime.Now,
                User = _mapper.Map<UserLookUp>(_userB),
                IsPinned = true,
                ChatId = _ids.PrivateChat3,
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