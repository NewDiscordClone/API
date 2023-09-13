using Application.Models;
using Application.Users.Queries.GetUserDetails;
using Tests.Common;

namespace Tests.Users.Queries
{
    public class GetUserDetailsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            GetUserDetailsRequest request = new()
            {
                UserId = userId
            };
            GetUserDetailsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            GetUserDetailsDto result = await handler.Handle(request, CancellationToken);

            //Assert
            User user = await Context.SqlUsers.FindAsync(userId);
            Assert.Equal(user.UserName, result.Username);
            Assert.Equal(user.DisplayName, result.DisplayName);
            Assert.Equal(user.Avatar, result.Avatar);
            Assert.Equal(user.Status, result.Status);
            Assert.Equal(user.TextStatus, result.TextStatus);
            Assert.Null(result.ServerProfile);
        }
        [Fact]
        public async Task Success_WithServer()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserBId;
            string serverId = Ids.Server3;

            SetAuthorizedUserId(Ids.UserAId);

            GetUserDetailsRequest request = new()
            {
                UserId = userId,
                ServerId = serverId
            };
            GetUserDetailsRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            GetUserDetailsDto result = await handler.Handle(request, CancellationToken);

            //Assert
            User user = await Context.SqlUsers.FindAsync(userId);
            ServerProfile? serverProfile = (await Context.Servers.FindAsync(serverId))
                .ServerProfiles
                .Find(sp => sp.UserId == userId);
            Assert.Equal(user.UserName, result.Username);
            Assert.Equal(user.DisplayName, result.DisplayName);
            Assert.Equal(user.Avatar, result.Avatar);
            Assert.Equal(user.Status, result.Status);
            Assert.Equal(user.TextStatus, result.TextStatus);
            Assert.NotNull(serverProfile);
            Assert.NotNull(result.ServerProfile);
            Assert.Equal(serverProfile.DisplayName, result.ServerProfile.DisplayName);
        }
    }
}