using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public class BlockUserCommandHandler : RequestHandlerBase, IRequestHandler<BlockUserCommand, Relationship>
    {
        private readonly IRelationshipRepository _relationshipRepository;

        public BlockUserCommandHandler(IAuthorizedUserProvider userProvider,
            IRelationshipRepository relationshipRepository) : base(userProvider)
        {
            _relationshipRepository = relationshipRepository;
        }

        public async Task<Relationship> Handle(BlockUserCommand command, CancellationToken cancellationToken)
        {
            Relationship? relationship = await _relationshipRepository
                .FindOrDefaultAsync((UserId, command.UserId), cancellationToken);

            if (relationship is not null)
            {
                if (relationship == RelationshipTypes.Blocked)
                    throw new InvalidOperationException("You already block this user");

                relationship.Active = UserId;
                relationship.Passive = command.UserId;
                relationship.RelationshipType = RelationshipTypes.Blocked;

                relationship = await _relationshipRepository.UpdateWithKeysAsync(relationship, cancellationToken);
            }
            else
            {
                relationship = new()
                {
                    Active = UserId,
                    Passive = command.UserId,
                    RelationshipType = RelationshipTypes.Blocked
                };

                await _relationshipRepository.AddAsync(relationship, cancellationToken);
            }

            return relationship;
        }
    }
}
