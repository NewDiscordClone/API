using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class AcceptFriendRequestCommandHandler : RequestHandlerBase, IRequestHandler<AcceptFriendRequestCommand, Relationship>
    {
        private readonly IRelationshipRepository _relationshipRepository;
        public AcceptFriendRequestCommandHandler(IAuthorizedUserProvider userProvider, IRelationshipRepository relationshipRepository)
            : base(userProvider)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task<Relationship> Handle(AcceptFriendRequestCommand command, CancellationToken cancellationToken)
        {
            Relationship relationship = await _relationshipRepository
                .FindAsync((UserId, command.FriendId), cancellationToken);

            if (relationship != RelationshipTypes.Pending)
                throw new InvalidOperationException("Friend request is not pending");

            if (UserId != relationship.Passive)
                throw new NoPermissionsException("You can't accept your own friend request");

            relationship.RelationshipType = RelationshipTypes.Friend;

            await _relationshipRepository.UpdateAsync(relationship, cancellationToken);

            return relationship;
        }
    }
}