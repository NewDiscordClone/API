using Application.Commands.PrivateChats.RemovePrivateChatMember;
using Application.Exceptions;
using Application.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class RemovePrivateChatMemberTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            CreateDatabase();
            //Arrange
            var chatId = Ids.PrivateChat6;
            ObjectId removeMemberId = Ids.UserAId;
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
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(oldCount - 1, chat.Users.Count);
            Assert.DoesNotContain(chat.Users, user => user.Id == removeMemberId);
        }

        [Fact]
        public async Task UserNotInChat_Fail()
        {
            CreateDatabase();
            //Arrange
            var chatId = Ids.PrivateChat7;
            ObjectId removeMemberId = Ids.UserAId;

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
