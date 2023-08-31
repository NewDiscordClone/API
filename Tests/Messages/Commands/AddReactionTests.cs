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
            //Arrange
            CreateDatabase();
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
            await handler.Handle(request, CancellationToken);
            Reaction result = (await Context.Messages.FindAsync(messageId)).Reactions.Last();

            //Assert

            Assert.NotNull(result);
            Assert.Equal(emoji, result.Emoji);
            Assert.Equal(Ids.UserBId, result.User.Id);
        }
    }
}