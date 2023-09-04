using Application.Commands.GroupChats.RemoveGroupChatMember;
using Application.Common.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.GroupChats.Commands
{
    public class RemoveGroupChatMemberTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat6;
            Guid removeMemberId = Ids.UserAId;
            int oldCount = 4;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveGroupChatMemberRequest request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemoveGroupChatMemberRequestHandler handler =
                new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PersonalChat chat = await Context.PersonalChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldCount - 1, chat.Users.Count);
            Assert.DoesNotContain(chat.Users, user => user.Id == removeMemberId);
        }

        [Fact]
        public async Task UserNotInChat_Fail()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat7;
            Guid removeMemberId = Ids.UserAId;

            SetAuthorizedUserId(Ids.UserBId);

            RemoveGroupChatMemberRequest request = new()
            {
                ChatId = chatId,
                MemberId = removeMemberId
            };
            RemoveGroupChatMemberRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoSuchUserException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
