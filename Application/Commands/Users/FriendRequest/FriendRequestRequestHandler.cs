using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Users.FriendRequest
{
    public class FriendRequestRequestHandler : RequestHandlerBase, IRequestHandler<FriendRequestRequest, string?>
    {
        public FriendRequestRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }

        public async Task<string?> Handle(FriendRequestRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            User user = await Context.SqlUsers.FindAsync(UserId);
            User other = await Context.SqlUsers.FindAsync(request.UserId);

            RelationshipList? userRelationship =
                await Context.RelationshipLists.FindAsync(UserId);
            RelationshipList? otherRelationship =
                await Context.RelationshipLists.FindAsync(request.UserId);
            Relationship? otherToUser = otherRelationship.Relationships.Find(r => r.UserId == UserId);
            Relationship? userToOther = userRelationship.Relationships.Find(r => r.UserId == request.UserId);
            //TODO: Додати реалізацію перевірки налаштуваннь користувача з дозволів відправляти запити дружби
            Chat? chat = null;
            switch (otherToUser)
            {
                case { RelationshipType: RelationshipType.Blocked }:
                    throw new NoPermissionsException("You are blocked from this user");
                case null:
                    otherRelationship.Relationships.Add(new Relationship
                    {
                        UserId = UserId,
                        RelationshipType = RelationshipType.Pending
                    });
                    await Context.RelationshipLists.UpdateAsync(otherRelationship);
                    chat = await Context.PersonalChats.AddAsync(new PersonalChat
                    {
                        Users = new List<UserLookUp>
                        {
                            Mapper.Map<UserLookUp>(user),
                            Mapper.Map<UserLookUp>(other),
                        }
                    });
                    break;
                default:
                    otherToUser.RelationshipType = RelationshipType.Pending;
                    await Context.RelationshipLists.UpdateAsync(otherRelationship);
                    break;
            }

            if (userToOther == null)
                userRelationship.Relationships.Add(new Relationship
                {
                    UserId = request.UserId,
                    RelationshipType = RelationshipType.Waiting
                });

            else
                userToOther.RelationshipType = RelationshipType.Waiting;
            await Context.RelationshipLists.UpdateAsync(userRelationship);

            return chat == null ? null : chat.Id;
        }
    }
}