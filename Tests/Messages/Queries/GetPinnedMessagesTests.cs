using Application.Models;
using Application.Queries.GetPinnedMessages;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Messages.Queries
{
    public class GetPinnedMessagesTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var chatId = TestDbContextFactory.PrivateChat3;

            GetPinnedMessagesRequest request = new()
            {
                ChatId = chatId
            };
            GetPinnedMessagesRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<Message> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.True(result.All(message =>
                Context.Messages.Find(Context.GetIdFilter<Message>(message.Id)).FirstOrDefault().IsPinned));
        }
    }
}