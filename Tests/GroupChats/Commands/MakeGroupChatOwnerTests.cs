using Application.Commands.GroupChats.MakeGroupChatOwner;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.GroupChats.Commands
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

            MakeGroupChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakeGroupChatOwnerRequestHandler handler =
                new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            GroupChat chat = await Context.GroupChats.FindAsync(chatId);

            //Assert
            GroupChat groupChat = chat as GroupChat;
            Assert.NotNull(groupChat);
            Assert.Equal(newOwnerId, groupChat.OwnerId);
        }

        [Fact]
        public async Task Fail_NoPermissions()
        {
            //Arrange
            CreateDatabase();

            string chatId = Ids.GroupChat6;
            Guid newOwnerId = Ids.UserCId;

            SetAuthorizedUserId(Ids.UserAId);

            MakeGroupChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakeGroupChatOwnerRequestHandler handler =
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

            MakeGroupChatOwnerRequest request = new()
            {
                ChatId = chatId,
                MemberId = newOwnerId
            };

            MakeGroupChatOwnerRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
