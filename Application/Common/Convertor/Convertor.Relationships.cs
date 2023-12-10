using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;
using Sparkle.Application.Users.Relationships.Queries;

namespace Sparkle.Application.Common.Convertor
{
    public partial class Convertor : IConvertor
    {
        private readonly IAppDbContext _context;
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper _mapper;

        public Convertor(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IAppDbContext context)
        {
            _userProvider = userProvider;
            _mapper = mapper;
            _context = context;
        }

        public RelationshipViewModel Convert(Relationship relationship, Guid? userId = null)
        {
            userId ??= _userProvider.GetUserId();

            User? user = _context.Users
                 .Find(relationship.Active == userId
                 ? relationship.Passive : relationship.Active);

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
