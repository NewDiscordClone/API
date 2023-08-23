using Application.Commands.Messages.AddReaction;
using Application.Models;
using Tests.Common;

namespace Tests.Messages.Commands
{
    public class AddReactionTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            CreateDatabase();
            //Arrange
            var messageId = Ids.Message1;
            const string emoji = "☻";

            SetAuthorizedUserId(Ids.UserBId);

            AddReactionRequest request = new()
            {
                Emoji = emoji,
                MessageId = messageId
            };

            AddReactionRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Reaction result = await handler.Handle(request, CancellationToken);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(emoji, result.Emoji);
        }
    }
}