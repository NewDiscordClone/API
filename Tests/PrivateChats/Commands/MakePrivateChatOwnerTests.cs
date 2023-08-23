using Application.Commands.PrivateChats.MakePrivateChatOwner;
using Application.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class MakePrivateChatOwnerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange

            var chatId = TestDbContextFactory.PrivateChat3;
            int newOwnerId = TestDbContextFactory.UserBId;
            int oldOwner = TestDbContextFactory.UserAId;

            SetAuthorizedUserId(oldOwner);

            MakePrivateChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakePrivateChatOwnerRequestHandler handler =
                new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(newOwnerId, chat.OwnerId);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange

            var chatId = TestDbContextFactory.PrivateChat6;
            int newOwnerId = TestDbContextFactory.UserCId;

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            MakePrivateChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakePrivateChatOwnerRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }

        [Fact]
        public async Task Fail_NoSuchUser()
        {
            //Arrange

            var chatId = TestDbContextFactory.PrivateChat7;
            int newOwnerId = TestDbContextFactory.UserAId;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            MakePrivateChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakePrivateChatOwnerRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
