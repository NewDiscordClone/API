using Application.Channels.Commands.RenameChannel;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Channels.Commands
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

            RenameChannelRequest request = new()
            {
                ChatId = channelId,
                NewTitle = newTitle,
            };
            RenameChannelRequestHandler handler = new(Context, UserProvider);

            //Act

            await handler.Handle(request, CancellationToken);
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

            RenameChannelRequest request = new()
            {
                ChatId = channelId,
                NewTitle = newTitle,
            };
            RenameChannelRequestHandler handler = new(Context, UserProvider);

            //Act
            //Assert

            await Assert.ThrowsAsync<NoPermissionsException>(async () =>
                await handler.Handle(request, CancellationToken));
            Channel channel = await Context.Channels.FindAsync(channelId);
            Assert.NotEqual(newTitle, channel.Title);
        }
    }
}