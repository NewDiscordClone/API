using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class LeaveFromPrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success_Owner()
        {
            //Arrange
            int chatId = 7;
            int userId = TestDbContextFactory.UserBId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(chatId);

            //Assert
            Assert.NotNull(chat);
            Assert.DoesNotContain(chat.Users, user => user.Id == chatId);
            Assert.NotEqual(userId, chat.OwnerId);
        }

        [Fact]
        public async Task Success_LastUser()
        {
            //Arrange
            int chatId = 4;
            int userId = TestDbContextFactory.UserAId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(chatId);

            //Assert
            Assert.Null(chat);
        }
        [Fact]
        public async Task Fail_NoSuchUser()
        {
            //Arrange
            int chatId = 4;
            int userId = TestDbContextFactory.UserBId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));


        }
    }
}
