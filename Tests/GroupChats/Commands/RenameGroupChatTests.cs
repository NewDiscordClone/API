using Application.Commands.GroupChats.RenameGroupChat;
using Application.Models;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.GroupChats.Commands
{
    public class RenameGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat4;
            string newTitle = "New title test";

            SetAuthorizedUserId(Ids.UserAId);

            RenameGroupChatRequest request = new()
            {
                ChatId = chatId,
                NewTitle = newTitle
            };

            RenameGroupChatRequestHandler handler = new(Context, UserProvider);

            //Act
            Context.SetToken(CancellationToken);
            await handler.Handle(request, CancellationToken);
            GroupChat chat = await Context.GroupChats.FindAsync(chatId);

            //Assert
            Assert.Equal(newTitle, chat.Title);
        }
    }
}
