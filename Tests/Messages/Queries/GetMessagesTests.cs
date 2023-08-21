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
            int chatId = 3;
            int messagesCount = 2;

            GetMessagesRequest request = new()
            {
                ChatId = chatId,
                MessagesCount = messagesCount
            };

            GetMessagesRequestHandler handler = new(Context, Mapper);

            //Act
            List<GetMessageLookUpDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SingleMessage()
        {
            //Arrange
            int chatId = 3;
            int messagesCount = 1;

            GetMessagesRequest request = new()
            {
                ChatId = chatId,
                MessagesCount = messagesCount
            };

            GetMessagesRequestHandler handler = new(Context, Mapper);

            //Act
            List<GetMessageLookUpDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Single(result);
        }
    }
}
