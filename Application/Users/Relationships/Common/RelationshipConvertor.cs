using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Relationships.Queries.GetRelationships;

namespace Sparkle.Application.Users.Relationships.Common
{
    public class RelationshipConvertor : IRelationshipConvertor
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper _mapper;

        public RelationshipConvertor(IRepository<User, Guid> userRepository,
            IAuthorizedUserProvider userProvider,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userProvider = userProvider;
            _mapper = mapper;
        }

        public RelationshipViewModel Convert(Relationship relationship)
        {
            Guid userId = _userProvider.GetUserId();

            User? user = _userRepository
                 .FindAsync(relationship.Active == userId
                 ? relationship.Passive : relationship.Active)
                 .Result;

            return new RelationshipViewModel
            {
                IsActive = relationship.Active != userId,
                User = _mapper.Map<UserLookupViewModel>(user),
                Type = relationship.RelationshipType
            };
        }
    }
}
