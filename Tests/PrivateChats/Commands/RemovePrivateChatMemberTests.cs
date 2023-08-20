using Application.Commands.PrivateChats.RemovePrivateChatMember;
using Application.Models;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class RemovePrivateChatMemberTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int chatId = 6;
            int removeMemberId = TestDbContextFactory.UserAId;
            int oldCount = 4;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            RemovePrivateChatMemberRequest request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemovePrivateChatMemberRequestHandler handler =
                new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(chatId);

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(oldCount - 1, chat.Users.Count);
            Assert.DoesNotContain(chat.Users, user => user.Id == removeMemberId);
        }

        [Fact]
        public async Task UserNotInChat_Fail()
        {
            //Arrange
            int chatId = 7;
            int removeMemberId = TestDbContextFactory.UserAId;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            RemovePrivateChatMemberRequest request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemovePrivateChatMemberRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async ()
                => await handler.Handle(request, CancellationToken));

        }
    }
}
