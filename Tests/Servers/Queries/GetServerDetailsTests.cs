﻿using Application.Providers;
using Application.Queries.GetServerDetails;
using Tests.Common;

namespace Tests.Servers.Queries
{
    public class GetServerDetailsTests : TestBase
    {
        [Fact]
        public async Task GetServerDetails_Expected()
        {
            CreateDatabase();
            //Arrange
            int serverId = Ids.ServerIdForDelete;
            int userId = Ids.UserAId;
            CancellationToken cancellationToken = CancellationToken.None;
            
            SetAuthorizedUserId(userId);
            
            GetServerDetailsRequestHandler handler = new(Context, UserProvider, Mapper);

            GetServerDetailsRequest request = new() { ServerId = serverId };

            //Act
            ServerDetailsDto result = await handler.Handle(request, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(serverId, result.Id);
            Assert.NotEmpty(result.Channels);

        }
    }
}
