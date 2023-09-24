using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.GroupChats.Commands
{
    public class RemoveGroupChatMemberTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat6;
            Guid removeMemberId = Ids.UserAId;
            int oldCount = 4;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveGroupChatMemberCommand request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemoveGroupChatMemberCommandHandler handler =
                new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldCount - 1, chat.Profiles.Count);
            Assert.DoesNotContain(chat.Profiles, profile => profile.UserId == removeMemberId);
        }

        [Fact]
        public async Task UserNotInChat_Fail()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat7;
            Guid removeMemberId = Ids.UserAId;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveGroupChatMemberCommand request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemoveGroupChatMemberCommandHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
