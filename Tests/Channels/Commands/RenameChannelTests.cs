using Sparkle.Application.Channels.Commands.RenameChannel;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Channels.Commands
{
    public class RenameChannelTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string channelId = Ids.Channel3;
            const string newTitle = "NewTitle";

            SetAuthorizedUserId(Ids.UserCId);

            RenameChannelCommand command = new()
            {
                ChatId = channelId,
                NewTitle = newTitle,
            };
            RenameChannelCommandHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(command, CancellationToken);
            Channel channel = await Context.Channels.FindAsync(channelId);

            //Assert
            Assert.Equal(newTitle, channel.Title);
        }
        [Fact]
        public async Task Fail_UserIsNotAMember()
        {
            //Arrange
            CreateDatabase();
            string channelId = Ids.Channel3;
            const string newTitle = "NewTitle";

            SetAuthorizedUserId(Ids.UserDId);

            RenameChannelCommand request = new()
            {
                ChatId = channelId,
                NewTitle = newTitle,
            };
            RenameChannelCommandHandler handler = new(Context, UserProvider);

            //Act
            //Assert

            await Assert.ThrowsAsync<NoPermissionsException>(async () =>
                await handler.Handle(request, CancellationToken));
            Channel channel = await Context.Channels.FindAsync(channelId);
            Assert.NotEqual(newTitle, channel.Title);
        }
    }
}