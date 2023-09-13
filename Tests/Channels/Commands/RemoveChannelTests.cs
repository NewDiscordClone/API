using Application.Commands.Channels.RemoveChannel;
using Application.Common.Exceptions;
using Tests.Common;

namespace Tests.Channels.Commands
{
    public class RemoveChannelTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string channelId = Ids.Channel3;
            string serverId = Ids.Server3;
            long oldCount = await Context.Servers.CountAsync(s => true);

            SetAuthorizedUserId(Ids.UserCId);
            RemoveChannelRequest request = new()
            {
                ChatId = channelId
            };
            RemoveChannelRequestHandler handler = new(Context, UserProvider);
            //Act
            await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await Context.Channels.FindAsync(channelId));
            Assert.Empty(await Context.Channels.FilterAsync(c => c.ServerId == serverId));
            Assert.Equal(oldCount - 1, await Context.Channels.CountAsync(s => true));
        }

        [Fact]
        public async Task Fail_UserIsNotAMember()
        {
            //Arrange
            CreateDatabase();
            string channelId = Ids.Channel3;
            string serverId = Ids.Server3;
            long oldCount = await Context.Servers.CountAsync(s => true);

            SetAuthorizedUserId(Ids.UserDId);
            RemoveChannelRequest request = new()
            {
                ChatId = channelId
            };
            RemoveChannelRequestHandler handler = new(Context, UserProvider);
            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Assert.NotNull(await Context.Channels.FindAsync(channelId));
            Assert.Single(await Context.Channels.FilterAsync(c => c.ServerId == serverId));
            Assert.Equal(oldCount, await Context.Channels.CountAsync(s => true));
        }
    }
}