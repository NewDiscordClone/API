using Application.Commands.PrivateChats.AddMemberToPrivateChat;
using Application.Exceptions;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class AddMemberToPrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange

            int newMemberId = TestDbContextFactory.UserAId;
            var chatId = TestDbContextFactory.PrivateChat5;
            int oldUsersCount = 2;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            AddMemberToPrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToPrivateChatRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();

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
            var chatId = TestDbContextFactory.PrivateChat5;

            SetAuthorizedUserId(TestDbContextFactory.UserBId);

            AddMemberToPrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToPrivateChatRequestHandler handler =
                new(Context, UserProvider, Mapper);

            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(async ()
                => await handler.Handle(request, CancellationToken));
        }
    }
}
