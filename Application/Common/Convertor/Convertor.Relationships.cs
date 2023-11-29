using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Relationships.Queries;

namespace Sparkle.Application.Common.Convertor
{
    public partial class Convertor : IConvertor
    {
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public Convertor(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IUserProfileRepository userProfileRepository,
            IUserRepository userRepository)
        {
            _userProvider = userProvider;
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
            _userRepository = userRepository;
        }

        public RelationshipViewModel Convert(Relationship relationship, Guid? userId = null)
        {
            userId ??= _userProvider.GetUserId();

            User? user = _userRepository.FindAsync(relationship.Active == userId
                 ? relationship.Passive : relationship.Active).Result;

            return new RelationshipViewModel
            {
                IsActive = relationship.Active != userId,
                User = _mapper.Map<UserLookupViewModel>(user),
                Type = relationship.RelationshipType,
                ChatId = relationship.PersonalChatId
            };
        }
    }
}
