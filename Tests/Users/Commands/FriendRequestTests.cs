using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Commands.FriendRequest;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Users.Commands
{
    public class FriendRequestTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            Guid otherId = Ids.UserCId;

            SetAuthorizedUserId(userId);

            CreateFriendRequestCommand request = new()
            {
                UserId = otherId
            };
            CreateFriendRequestCommandHandler handler = new(Context, UserProvider);

            //Act
            string? chatId = await handler.Handle(request, CancellationToken);

            //Assert
            RelationshipList my = await Context.RelationshipLists.FindAsync(userId);
            RelationshipList other = await Context.RelationshipLists.FindAsync(otherId);
            Assert.NotEmpty(other.Relationships);

            Relationship? myToOther = my.Relationships.Find(r => r.UserId == otherId);
            Relationship? otherToMe = other.Relationships.Find(r => r.UserId == userId);
            Assert.NotNull(myToOther);
            Assert.NotNull(otherToMe);
            Assert.Equal(RelationshipType.Waiting, myToOther.RelationshipType);
            Assert.Equal(RelationshipType.Pending, otherToMe.RelationshipType);

            Assert.NotNull(chatId);
            Chat chat = await Context.Chats.FindAsync(chatId);
            Assert.Equal(2, chat.Profiles.Count);
            Assert.Contains(userId, chat.Profiles.Select(p => p.UserId));
            Assert.Contains(otherId, chat.Profiles.Select(p => p.UserId));
        }

        [Fact]
        public async Task Fail_Blocked()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            Guid otherId = Ids.UserDId;

            SetAuthorizedUserId(userId);

            CreateFriendRequestCommand request = new()
            {
                UserId = otherId
            };
            CreateFriendRequestCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));

        }
    }
}