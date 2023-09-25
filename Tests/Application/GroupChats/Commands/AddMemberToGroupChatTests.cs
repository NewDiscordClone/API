using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.GroupChats.Commands.AddMemberToGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.GroupChats.Commands
{
    public class AddMemberToGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();

            Guid newMemberId = Ids.UserAId;
            string chatId = Ids.GroupChat5;
            int oldUsersCount = 2;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToGroupChatCommand request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            Mock<IUserProfileRepository> mock = new();
            AddMemberToGroupChatCommandHandler handler = new(UserProvider, mock.Object);

            //Act

            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldUsersCount + 1, chat.Profiles.Count);
            Assert.Single(chat.Profiles,
                Context.UserProfiles.Where(profile => profile.UserId == newMemberId
                && profile.ChatId == chatId));
        }
        [Fact]
        public async Task UserAlreadyExists_Fail()
        {
            //Arrange
            CreateDatabase();
            Guid newMemberId = Ids.UserCId;
            string chatId = Ids.GroupChat5;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToGroupChatCommand request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            Mock<IUserProfileRepository> mock = new();
            AddMemberToGroupChatCommandHandler handler = new(UserProvider, mock.Object);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
