using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Common.Exceptions;
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
            CreateDatabase();
            var chatId = Ids.PrivateChat7;
            int userId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PrivateChat chat = await Context.PrivateChats.FindAsync(chatId);

            //Assert
            Assert.DoesNotContain(chat.Users, user => user.Id == userId);
            Assert.NotEqual(userId, chat.OwnerId);
        }

        [Fact]
        public async Task Success_LastUser()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat4;
            int userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            LeaveFromPrivateChatRequest request = new()
            {
                ChatId = chatId,
            };

            LeaveFromPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>( 
                async () => await Context.PrivateChats.FindAsync(chatId));
        }
        [Fact]
        public async Task Fail_NoSuchUser()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat4;
            int userId = Ids.UserBId;

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
