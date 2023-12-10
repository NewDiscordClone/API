using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Commands
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

            if (relationship is null || relationship != RelationshipTypes.Pending)
                throw new InvalidOperationException("No friend pending");

            if (relationship.PersonalChatId is null)
            {
                await _relationshipRepository.DeleteAsync(relationship, cancellationToken);
            }
            else
            {
                relationship.RelationshipType = RelationshipTypes.Acquaintance;
                await _relationshipRepository.UpdateAsync(relationship, cancellationToken);
            }

            return relationship;
        }
    }
}
