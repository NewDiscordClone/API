using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Users.AcceptFriendRequest
{
    public class AcceptFriendRequestRequestHandler : RequestHandlerBase, IRequestHandler<AcceptFriendRequestRequest>
    {
        public AcceptFriendRequestRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }

        public async Task Handle(AcceptFriendRequestRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            RelationshipList? userRelationship = await FindOrCreateRelationshipsAsync(UserId);
            RelationshipList? otherRelationship = await FindOrCreateRelationshipsAsync(request.UserId);
            
            Relationship? otherToUser = otherRelationship.Relationships.Find(r => r.UserId == UserId);
            Relationship? userToOther = userRelationship.Relationships.Find(r => r.UserId == request.UserId);
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