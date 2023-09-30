using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.DeleteFriend
{
    public class DeleteFriendCommandHandler : RequestHandlerBase, IRequestHandler<DeleteFriendCommand, Relationship>
    {
        private readonly IRelationshipRepository _relationshipsRepository;
        public DeleteFriendCommandHandler(IAuthorizedUserProvider userProvider, IRelationshipRepository relationshipsRepository) : base(userProvider)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task<Relationship> Handle(DeleteFriendCommand command, CancellationToken cancellationToken)
        {
            Relationship relationship = await _relationshipsRepository
                .FindAsync((UserId, command.FriendId), cancellationToken);

            if (relationship.RelationshipType != RelationshipTypes.Friend)
                throw new InvalidOperationException("You are not friends");

            await _relationshipsRepository.DeleteAsync(relationship, cancellationToken);

            return relationship;
        }
    }
}
