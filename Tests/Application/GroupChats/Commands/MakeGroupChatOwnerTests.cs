using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.GroupChats.Commands.ChangeGroupChatOwner;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.GroupChats.Commands
{
    public class MakeGroupChatOwnerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();

            string chatId = Ids.GroupChat3;
            Guid newOwnerId = Ids.UserBId;
            Guid oldOwner = Ids.UserAId;

            SetAuthorizedUserId(oldOwner);

            ChangeGroupChatOwnerCommand request = new()
            {
                ChatId = chatId,
                ProfileId = newOwnerId
            };

            ChangeGroupChatOwnerCommandHandler handler =
                new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            GroupChat chat = await Context.GroupChats.FindAsync(chatId);

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(newOwnerId, chat.OwnerId);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();

            string chatId = Ids.GroupChat6;
            Guid newOwnerId = Ids.UserCId;

            SetAuthorizedUserId(Ids.UserAId);

            ChangeGroupChatOwnerCommand request = new()
            {
                ChatId = chatId,
                ProfileId = newOwnerId
            };

            ChangeGroupChatOwnerCommandHandler handler =
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

            string chatId = Ids.GroupChat7;
            Guid newOwnerId = Ids.UserAId;

            SetAuthorizedUserId(Ids.UserBId);

            ChangeGroupChatOwnerCommand request = new()
            {
                ChatId = chatId,
                ProfileId = newOwnerId
            };

            ChangeGroupChatOwnerCommandHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
