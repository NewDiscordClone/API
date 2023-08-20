using Application.Commands.PrivateChats.MakePrivateChatOwner;
using Application.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class MakePrivateChatOwnerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange

            int chatId = 3;
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
            PrivateChat? chat = Context.PrivateChats.Find(chatId);

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(newOwnerId, chat.OwnerId);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange

            int chatId = 6;
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

            int chatId = 7;
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
