using Application.Commands.PrivateChats.MakePrivateChatOwner;
using Application.Common.Exceptions;
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
            CreateDatabase();

            var chatId = Ids.PrivateChat3;
            int newOwnerId = Ids.UserBId;
            int oldOwner = Ids.UserAId;

            SetAuthorizedUserId(oldOwner);

            MakePrivateChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakePrivateChatOwnerRequestHandler handler =
                new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PrivateChat chat = await Context.PrivateChats.FindAsync(chatId);

            //Assert
            Assert.Equal(newOwnerId, chat.OwnerId);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();

            var chatId = Ids.PrivateChat6;
            int newOwnerId = Ids.UserCId;

            SetAuthorizedUserId(Ids.UserAId);

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
            CreateDatabase();

            var chatId = Ids.PrivateChat7;
            int newOwnerId = Ids.UserAId;

            SetAuthorizedUserId(Ids.UserBId);

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
