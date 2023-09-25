using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.GroupChats.Commands.RemoveUserFromGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.GroupChats.Commands
{
    public class LeaveFromGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success_Owner()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat7;
            Guid userId = Ids.UserBId;
            Guid profileId = Guid.NewGuid();

            SetAuthorizedUserId(userId);

            RemoveUserFromGroupChatCommand request = new()
            {
                ChatId = chatId,
                ProfileId = profileId,
            };

            Mock<IUserProfileRepository> mock = new();

            RemoveUserFromGroupChatCommandHandler handler = new(Context, UserProvider, mock.Object);

            //Act

            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.DoesNotContain(chat.Profiles, profile => profile == profileId);
            GroupChat? groupChat = chat as GroupChat;
            Assert.NotNull(groupChat);
            Assert.NotEqual(userId, groupChat.OwnerId);
        }

        [Fact]
        public async Task Success_LastUser()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat4;
            Guid userId = Ids.UserAId;
            Guid profileId = default;

            SetAuthorizedUserId(userId);

            RemoveUserFromGroupChatCommand request = new()
            {
                ChatId = chatId,
                ProfileId = profileId,
            };

            Mock<IUserProfileRepository> mock = new();

            RemoveUserFromGroupChatCommandHandler handler = new(Context, UserProvider, mock.Object);

            //Act

            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await Context.PersonalChats.FindAsync(chatId));
        }

        [Fact]
        public async Task Fail_NoSuchUser()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat4;
            Guid userId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            Guid profileId = default;
            RemoveUserFromGroupChatCommand request = new()
            {
                ChatId = chatId,
                ProfileId = profileId,
            };

            Mock<IUserProfileRepository> mock = new();

            RemoveUserFromGroupChatCommandHandler handler = new(Context, UserProvider, mock.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
