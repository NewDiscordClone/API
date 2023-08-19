using Application.Commands.PrivateChats.AddMemberToPrivateChat;
using Application.Models;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class AddMemberToPrivateChatTests : TestQueryBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange

            int newMemberId = TestDbContextFactory.UserAId;
            int chatId = 5;
            int oldUsersCount = 2;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            AddMemberToPrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToPrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(chatId);

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(oldUsersCount + 1, chat.Users.Count);
            Assert.Contains(chat.Users, user => user.Id == newMemberId);
        }
        [Fact]
        public async Task UserAlreadyExists_Fail()
        {
            //Arrange
            int newMemberId = TestDbContextFactory.UserCId;
            int chatId = 5;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            AddMemberToPrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToPrivateChatRequestHandler handler =
                new(Context, UserProvider);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
