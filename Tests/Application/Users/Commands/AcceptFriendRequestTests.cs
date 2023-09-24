using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Commands.AcceptFriendRequest;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Users.Commands
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

            AcceptFriendRequestCommand request = new()
            {
                UserId = otherId
            };
            AcceptFriendRequestCommandHandler handler = new(Context, UserProvider);

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

            AcceptFriendRequestCommand request = new()
            {
                UserId = otherId
            };
            AcceptFriendRequestCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
            async () => await handler.Handle(request, CancellationToken));
        }
    }
}