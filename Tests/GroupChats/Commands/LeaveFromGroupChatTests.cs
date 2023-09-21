using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.GroupChats.Commands
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

            SetAuthorizedUserId(userId);

            LeaveFromGroupChatCommand request = new()
            {
                ChatId = chatId,
            };

            LeaveFromGroupChatCommandHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.DoesNotContain(chat.Profiles, profile => profile.UserId == userId);
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

            SetAuthorizedUserId(userId);

            LeaveFromGroupChatCommand request = new()
            {
                ChatId = chatId,
            };

            LeaveFromGroupChatCommandHandler handler = new(Context, UserProvider);

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

            LeaveFromGroupChatCommand request = new()
            {
                ChatId = chatId,
            };

            LeaveFromGroupChatCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
