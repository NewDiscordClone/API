using Application.Commands.Users.FriendRequest;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Users.Commands
{
    public class FriendRequestTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            string otherName = "user_c"; 
            Guid otherId = Ids.UserCId; 
            
            SetAuthorizedUserId(userId);

            FriendRequestRequest request = new()
            {
                UserName = otherName
            };
            FriendRequestRequestHandler handler = new(Context, UserProvider);

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
            Assert.Equal(2, chat.Users.Count);
            Assert.Contains(userId, chat.Users);
            Assert.Contains(otherId, chat.Users);
        }
        [Fact]
        public async Task Fail_Blocked()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            string otherName = "user_d"; 
            Guid otherId = Ids.UserDId; 
            
            SetAuthorizedUserId(userId);

            FriendRequestRequest request = new()
            {
                UserName = otherName
            };
            FriendRequestRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () =>await handler.Handle(request, CancellationToken));
            
        }
    }
}