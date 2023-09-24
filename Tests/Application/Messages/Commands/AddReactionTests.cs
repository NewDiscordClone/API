using Sparkle.Application.Messages.Commands.AddReaction;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Messages.Commands
{
    public class AddReactionTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string messageId = Ids.Message1;
            const string emoji = ":add-reaction-test:";

            SetAuthorizedUserId(Ids.UserBId);

            AddReactionCommand request = new()
            {
                Emoji = emoji,
                MessageId = messageId
            };

            AddReactionCommandHandler handler = new(Context, UserProvider, Mapper);

            //Act
            await handler.Handle(request, CancellationToken);
            List<Reaction> result = (await Context.Messages.FindAsync(messageId)).Reactions;

            //Assert

            Assert.NotEmpty(result);
            Assert.Contains(result, r => r.User == Ids.UserBId && r.Emoji == emoji);
        }
    }
}