using Application.Queries.GetMessageReactions;
using Tests.Common;

namespace Tests.Messages.Queries
{
    public class GetMessageReactionsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange

            int messageId = 1;
            GetMessageReactionsRequest request = new()
            {
                MessageId = messageId
            };
            GetMessageReactionsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<UserReactionDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.Equal(2, result.Count);
        }
    }
}
