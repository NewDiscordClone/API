using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Exceptions;
using Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class LeaveFromPrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success_Owner()
        {
            CreateDatabase();
            //Arrange
            var chatId = Ids.PrivateChat7;
            ObjectId userId = Ids.UserBId;

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
            CreateDatabase();
            //Arrange
            var chatId = Ids.PrivateChat4;
            ObjectId userId = Ids.UserAId;

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
            CreateDatabase();
            //Arrange
            var chatId = Ids.PrivateChat4;
            ObjectId userId = Ids.UserBId;

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
