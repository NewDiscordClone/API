using Application.Queries.GetPinnedMessages;
using Tests.Common;

namespace Tests.Messages.Queries
{
    public class GetPinnedMessagesTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            int chatId = 3;

            GetPinnedMessagesRequest request = new()
            {
                ChatId = chatId
            };
            GetPinnedMessagesRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<GetPinnedMessageLookUpDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.True(result.All(message =>
          Context.Messages.Any(msg => msg.Id == message.Id && msg.IsPinned)));

        }
    }
}
