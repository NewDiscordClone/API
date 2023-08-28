using Application.Commands.PrivateChats.RenamePrivateChat;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class RenamePrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.PrivateChat4;
            string newTitle = "New title test";

            SetAuthorizedUserId(Ids.UserAId);

            RenamePrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewTitle = newTitle
            };

            RenamePrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(Context.GetIdFilter<PrivateChat>(chatId)).FirstOrDefault();

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(newTitle, chat.Title);
        }
    }
}
