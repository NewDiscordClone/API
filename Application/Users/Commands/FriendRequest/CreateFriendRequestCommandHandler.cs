﻿using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands.FriendRequest
{
    public class CreateFriendRequestCommandHandler : RequestHandlerBase, IRequestHandler<CreateFriendRequestCommand, string?>
    {
        public CreateFriendRequestCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }

        public async Task<string?> Handle(CreateFriendRequestCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            RelationshipList userRelationship = await FindOrCreateRelationshipsAsync(UserId);
            RelationshipList otherRelationship = await FindOrCreateRelationshipsAsync(request.UserId);

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
                        Users = new List<Guid>
                        {
                            UserId,
                            request.UserId,
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

            return chat?.Id;
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