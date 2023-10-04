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

            if (relationship != null)
                throw new InvalidOperationException("Relationship already exists");

            if (UserId == command.FriendId)
                throw new InvalidOperationException("You can't add yourself");

            relationship = new Relationship
            {
                Active = UserId,
                Passive = command.FriendId,
                RelationshipType = RelationshipTypes.Pending
            };

            await _relationshipRepository.AddAsync(relationship, cancellationToken);

            return relationship;
        }
    }
}