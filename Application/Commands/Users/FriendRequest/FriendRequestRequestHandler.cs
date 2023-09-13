﻿using Amazon.Runtime.Internal;
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

            RelationshipList userRelationship = await FindOrCreateRelationshipsAsync(UserId);
            User? user = (await Context.SqlUsers.FilterAsync(u => u.UserName == request.UserName)).FirstOrDefault();
            if (user == null) throw new EntityNotFoundException("The provided user isn't exist");
            RelationshipList otherRelationship = await FindOrCreateRelationshipsAsync(user.Id);
            
            Relationship? otherToUser = otherRelationship.Relationships.Find(r => r.UserId == UserId);
            Relationship? userToOther = userRelationship.Relationships.Find(r => r.UserId == user.Id);
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
                            user.Id,
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
                    UserId = user.Id,
                    RelationshipType = RelationshipType.Waiting
                });

            else
                userToOther.RelationshipType = RelationshipType.Waiting;
            await Context.RelationshipLists.UpdateAsync(userRelationship);

            return chat == null ? null : chat.Id;
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