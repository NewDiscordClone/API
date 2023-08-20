using Application.Commands.PrivateChats.RenamePrivateChat;
using Application.Models;
using Tests.Common;

namespace Tests.PrivateChats.Commands
{
    public class RenamePrivateChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int chatId = 4;
            string newTitle = "New title test";

            SetAuthorizedUserId(TestDbContextFactory.UserAId);

            RenamePrivateChatRequest request = new()
            {
                ChatId = chatId,
                NewTitle = newTitle
            };

            RenamePrivateChatRequestHandler handler = new(Context, UserProvider);

            //Act
            await handler.Handle(request, CancellationToken);
            PrivateChat? chat = Context.PrivateChats.Find(chatId);

            //Assert
            Assert.NotNull(chat);
            Assert.Equal(newTitle, chat.Title);
        }
    }
}
