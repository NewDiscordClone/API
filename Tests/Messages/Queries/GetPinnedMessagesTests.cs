using Application.Messages.Queries.GetPinnedMessages;
using Application.Models;
using Tests.Common;

namespace Tests.Messages.Queries
{
    public class GetPinnedMessagesTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string chatId = Ids.GroupChat3;

            SetAuthorizedUserId(Ids.UserAId);

            GetPinnedMessagesRequest request = new()
            {
                ChatId = chatId
            };
            GetPinnedMessagesRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act

            List<Message> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.True(result.All(message => Context.Messages.FindAsync(message.Id).Result.IsPinned));
        }
    }
}