using Application.Commands.Channels.CreateChannel;
using Application.Models;
using Tests.Common;

namespace Tests.Channels.Commands
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

            CreateChannelRequest request = new()
            {
                Title = title,
                ServerId = serverId
            };
            CreateChannelRequestHandler handler = new(Context, UserProvider, Mapper);
            //Act
            
            string channelId = await handler.Handle(request, CancellationToken);
            Server server = await Context.Servers.FindAsync(serverId);
            Channel channel = await Context.Channels.FindAsync(channelId);

            //Assert
            Assert.Equal(serverId, channel.ServerId);
            Assert.Equal(title, channel.Title);
            Assert.All(channel.Users, 
                user => Assert.Contains(server.ServerProfiles, sp => sp.UserId == user));
            
            
        }
    }
}