using Sparkle.Application.Channels.Commands.CreateChannel;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Channels.Commands
{
    public class CreateChannelTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server3;
            const string title = "TestCreate";

            SetAuthorizedUserId(Ids.UserAId);

            CreateChannelCommand request = new()
            {
                Title = title,
                ServerId = serverId
            };
            CreateChannelCommandHandler handler = new(Context, UserProvider, Mapper);
            //Act

            string channelId = await handler.Handle(request, CancellationToken);
            Server server = await Context.Servers.FindAsync(serverId);
            Channel channel = await Context.Channels.FindAsync(channelId);

            //Assert
            Assert.Equal(serverId, channel.ServerId);
            Assert.Equal(title, channel.Title);
            Assert.All(channel.Profiles,
                profile => Assert.Contains(server.ServerProfiles, sp => sp.Id == profile.Id));


        }
    }
}