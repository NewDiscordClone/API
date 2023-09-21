using Sparkle.Application.GroupChats.Commands.RenameGroupChat;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.GroupChats.Commands
{
    public class RenameGroupChatTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat4;
            string newTitle = "New title test";

            SetAuthorizedUserId(Ids.UserAId);

            RenameGroupChatCommand request = new()
            {
                ChatId = chatId,
                NewTitle = newTitle
            };

            RenameGroupChatCommandHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
            GroupChat chat = await Context.GroupChats.FindAsync(chatId);

            //Assert
            Assert.Equal(newTitle, chat.Title);
        }
    }
}
