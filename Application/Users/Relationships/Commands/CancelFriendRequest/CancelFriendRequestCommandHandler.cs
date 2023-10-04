using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands.CancelFriendRequest
{
    public class CancelFriendRequestCommandHandler : RequestHandlerBase, IRequestHandler<CancelFriendRequestCommand, Relationship>
    {
        private readonly IRelationshipRepository _relationshipRepository;

        public CancelFriendRequestCommandHandler(IRelationshipRepository relationshipRepository, IAuthorizedUserProvider userProvider)
            : base(userProvider)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task<Relationship> Handle(CancelFriendRequestCommand command, CancellationToken cancellationToken)
        {
            Relationship? relationship = await _relationshipRepository.FindOrDefaultAsync((command.FriendId, UserId), cancellationToken);

            if (relationship is null || relationship.RelationshipType != RelationshipTypes.Pending)
                throw new InvalidOperationException("No friend pending");

            await _relationshipRepository.DeleteAsync(relationship, cancellationToken);
            return relationship;
        }
    }
}
