using Application.Commands.PrivateChats.RemovePrivateChatMember;
using Application.Common.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class RemovePrivateChatMemberTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat6;
            int removeMemberId = Ids.UserAId;
            int oldCount = 4;

            SetAuthorizedUserId(Ids.UserBId);

            RemovePrivateChatMemberRequest request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemovePrivateChatMemberRequestHandler handler =
                new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PrivateChat chat = await Context.PrivateChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldCount - 1, chat.Users.Count);
            Assert.DoesNotContain(chat.Users, user => user.Id == removeMemberId);
        }

        [Fact]
        public async Task UserNotInChat_Fail()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat7;
            int removeMemberId = Ids.UserAId;

            SetAuthorizedUserId(Ids.UserBId);

            RemovePrivateChatMemberRequest request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemovePrivateChatMemberRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
