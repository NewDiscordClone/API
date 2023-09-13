﻿using Application.Common.Exceptions;
using Application.Models;
using Application.Users.Commands.AcceptFriendRequest;
using Tests.Common;

namespace Tests.Users.Commands
{
    public class AcceptFriendRequestTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserDId;
            Guid otherId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            AcceptFriendRequestRequest request = new()
            {
                UserId = otherId
            };
            AcceptFriendRequestRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            RelationshipList my = await Context.RelationshipLists.FindAsync(userId);
            RelationshipList other = await Context.RelationshipLists.FindAsync(otherId);
            Assert.NotEmpty(my.Relationships);
            Assert.NotEmpty(other.Relationships);

            Relationship? myToOther = my.Relationships.Find(r => r.UserId == otherId);
            Relationship? otherToMe = other.Relationships.Find(r => r.UserId == userId);
            Assert.NotNull(myToOther);
            Assert.NotNull(otherToMe);
            Assert.Equal(RelationshipType.Friend, myToOther.RelationshipType);
            Assert.Equal(RelationshipType.Friend, otherToMe.RelationshipType);
        }
        [Fact]
        public async Task Fail_NoPending()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            Guid otherId = Ids.UserCId;

            SetAuthorizedUserId(userId);

            AcceptFriendRequestRequest request = new()
            {
                UserId = otherId
            };
            AcceptFriendRequestRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
            async () => await handler.Handle(request, CancellationToken));
        }
    }
}