using Application.Models;
using Application.Queries.GetMessages;
using Tests.Common;

namespace Tests.Messages.Queries
{
    public class GetMessagesTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat3;
            int messagesCount = 2;

            GetMessagesRequest request = new()
            {
                ChatId = chatId,
                MessagesCount = messagesCount
            };

            GetMessagesRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<Message> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SingleMessage()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat3;
            int messagesCount = 1;

            GetMessagesRequest request = new()
            {
                ChatId = chatId,
                MessagesCount = messagesCount
            };

            GetMessagesRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<Message> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Single(result);
        }
    }
}
