using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class UnblockUserCommandHandler : RequestHandlerBase, IRequestHandler<UnblockUserCommand, Relationship>
    {
        private readonly IRelationshipRepository _relationshipRepository;

        public UnblockUserCommandHandler(IAuthorizedUserProvider userProvider,
            IRelationshipRepository relationshipRepository) : base(userProvider)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task<Relationship> Handle(UnblockUserCommand command, CancellationToken cancellationToken)
        {
            Relationship relationship = await _relationshipRepository
                .FindAsync((UserId, command.UserId), cancellationToken);

            if (relationship != RelationshipTypes.Blocked || relationship.Active != UserId)
                throw new InvalidOperationException("You cand unblock this user");

            if (relationship.PersonalChatId is not null)
            {
                relationship.RelationshipType = RelationshipTypes.Acquaintance;
                await _relationshipRepository.UpdateAsync(relationship, cancellationToken);
            }
            else
            {
                await _relationshipRepository.DeleteAsync(relationship, cancellationToken);
            }

            return relationship;
        }
    }
}
