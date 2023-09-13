using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Commands.SendMessageToUser;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Users.Commands
{
    public class SendMessageToUserTests : TestBase
    {
        [Fact]
        public async Task Success_CreateChat()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserCId;
            Guid otherId = Ids.UserBId;
            string messageText = "Test Message Text";

            SetAuthorizedUserId(userId);

            SendMessageToUserRequest request = new()
            {
                UserId = otherId,
                Attachments = new List<Attachment>(),
                Text = messageText
            };
            SendMessageToUserRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            MessageChatDto messageChatDto = await handler.Handle(request, CancellationToken);

            //Assert
            RelationshipList my = await Context.RelationshipLists.FindAsync(userId);
            RelationshipList other = await Context.RelationshipLists.FindAsync(otherId);
            Assert.NotEmpty(my.Relationships);

            Relationship? myToOther = my.Relationships.Find(r => r.UserId == otherId);
            Relationship? otherToMe = other.Relationships.Find(r => r.UserId == userId);
            Assert.NotNull(myToOther);
            Assert.NotNull(otherToMe);
            Assert.Equal(RelationshipType.Acquaintance, myToOther.RelationshipType);
            Assert.Equal(RelationshipType.Acquaintance, otherToMe.RelationshipType);

            Chat chat = await Context.Chats.FindAsync(messageChatDto.ChatId);
            Assert.Equal(2, chat.Users.Count);
            Assert.Contains(userId, chat.Users);
            Assert.Contains(otherId, chat.Users);

            Message message = await Context.Messages.FindAsync(messageChatDto.MessageId);
            Assert.Equal(chat.Id, message.ChatId);
            Assert.Equal(messageText, message.Text);
        }
        [Fact]
        public async Task Fail_YouAlreadyHaveChat()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            Guid otherId = Ids.UserBId;
            string messageText = "Test Message Text";

            SetAuthorizedUserId(userId);

            SendMessageToUserRequest request = new()
            {
                UserId = otherId,
                Attachments = new List<Attachment>(),
                Text = messageText
            };
            SendMessageToUserRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(
                async () => await handler.Handle(request, CancellationToken));
        }

        [Fact]
        public async Task Fail_Blocked()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;
            Guid otherId = Ids.UserDId;
            string messageText = "Test Message Text";

            SetAuthorizedUserId(userId);

            SendMessageToUserRequest request = new()
            {
                UserId = otherId,
                Attachments = new List<Attachment>(),
                Text = messageText
            };
            SendMessageToUserRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
        }
    }
}