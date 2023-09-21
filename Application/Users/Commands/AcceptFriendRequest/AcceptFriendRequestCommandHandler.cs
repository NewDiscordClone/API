using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandHandler : RequestHandlerBase, IRequestHandler<AcceptFriendRequestCommand>
    {
        public AcceptFriendRequestCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }

        public async Task Handle(AcceptFriendRequestCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            RelationshipList? userRelationship = await FindOrCreateRelationshipsAsync(UserId);
            RelationshipList? otherRelationship = await FindOrCreateRelationshipsAsync(command.UserId);

            Relationship? otherToUser = otherRelationship.Relationships.Find(r => r.UserId == UserId);
            Relationship? userToOther = userRelationship.Relationships.Find(r => r.UserId == command.UserId);
            if (userToOther is { RelationshipType: RelationshipType.Pending } &&
                otherToUser is { RelationshipType: RelationshipType.Waiting })
            {
                otherToUser.RelationshipType = RelationshipType.Friend;
                userToOther.RelationshipType = RelationshipType.Friend;
                await Context.RelationshipLists.UpdateAsync(userRelationship);
                await Context.RelationshipLists.UpdateAsync(otherRelationship);
            }
            else
                throw new NoPermissionsException("There is no pending to accept friend");
        }

        private async Task<RelationshipList> FindOrCreateRelationshipsAsync(Guid id)
        {
            try
            {
                return await Context.RelationshipLists.FindAsync(id);
            }
            catch (EntityNotFoundException)
            {
                return await Context.RelationshipLists.AddAsync(new RelationshipList()
                {
                    Id = id,
                    Relationships = new List<Relationship>()
                });
            }
        }
    }
}