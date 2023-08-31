using Application.Commands.PrivateChats.AddMemberToPrivateChat;
using Application.Common.Exceptions;
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
            CreateDatabase();

            int newMemberId = Ids.UserAId;
            var chatId = Ids.PrivateChat5;
            int oldUsersCount = 2;

            SetAuthorizedUserId(Ids.UserBId);

            AddMemberToPrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewMemberId = newMemberId
            };

            AddMemberToPrivateChatRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            PrivateChat chat =await Context.PrivateChats.FindAsync(chatId);

            //Assert
            Assert.Equal(oldUsersCount + 1, chat.Users.Count);
            Assert.Contains(chat.Users, user => user.Id == newMemberId);
        }
        [Fact]
        public async Task UserAlreadyExists_Fail()
        {
            //Arrange
            CreateDatabase();
            int newMemberId = Ids.UserCId;
            var chatId = Ids.PrivateChat5;

            SetAuthorizedUserId(Ids.UserBId);

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
