using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands.SendFriendRequest
{
    public class CreateFriendRequestCommandHandler : RequestHandlerBase, IRequestHandler<CreateFriendRequestCommand, Relationship>
    {
        private readonly IRelationshipRepository _relationshipRepository;
        public CreateFriendRequestCommandHandler(IRelationshipRepository relationshipRepository, IAuthorizedUserProvider userProvider)
            : base(userProvider)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task<Relationship> Handle(CreateFriendRequestCommand command, CancellationToken cancellationToken)
        {
            Relationship? relationship = await _relationshipRepository
                .FindOrDefaultAsync((UserId, command.FriendId), cancellationToken);
            bool isNull = relationship is null;

            if (relationship != null && relationship == RelationshipTypes.Blocked)
                throw new InvalidOperationException("You are blocked by this user");

            if (relationship != null && (relationship == RelationshipTypes.Friend
                    || relationship == RelationshipTypes.Pending))
            {
                throw new InvalidOperationException("You are already friends");
            }

            relationship = new Relationship
            {
                Active = UserId,
                Passive = command.FriendId,
                RelationshipType = RelationshipTypes.Pending,
                PersonalChatId = relationship?.PersonalChatId
            };

            if (isNull)
                await _relationshipRepository.AddAsync(relationship, cancellationToken);
            else
                await _relationshipRepository.UpdateWithKeysAsync(relationship, cancellationToken);

            return relationship;
        }
    }
}