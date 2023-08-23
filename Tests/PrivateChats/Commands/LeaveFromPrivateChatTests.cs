using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class LeaveFromPrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success_Owner()
        {
            //Arrange
            var chatId = TestDbContextFactory.PrivateChat7;
            int userId = TestDbContextFactory.UserBId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();

            //Assert
            Assert.NotNull(chat);
            Assert.DoesNotContain(chat.Users, user => user.Id == userId);
            Assert.NotEqual(userId, chat.OwnerId);
        }

        [Fact]
        public async Task Success_LastUser()
        {
            //Arrange
            var chatId = TestDbContextFactory.PrivateChat4;
            int userId = TestDbContextFactory.UserAId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();

            //Assert
            Assert.Null(chat);
        }
        [Fact]
        public async Task Fail_NoSuchUser()
        {
            //Arrange
            var chatId = TestDbContextFactory.PrivateChat4;
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
