using System.Globalization;
using System.Security.Policy;
using Application.Commands.Invitations.CreateInvitation;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Invitations
{
    public class CreateInvitationTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server1;
            DateTime expiretime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            Guid user = Ids.UserAId;
            
            SetAuthorizedUserId(user);
            
            CreateInvitationRequest request = new()
            {
                ServerId = serverId,
                IncludeUser = true,
                ExpireTime = expiretime
            };
            CreateInvitationRequestHandler handler = new(Context, UserProvider);
            
            //Act
            string invitationId = await handler.Handle(request, CancellationToken);
            Invitation invitation = await Context.Invitations.FindAsync(invitationId);
            
            //Assert
            Assert.Equal(serverId, invitation.ServerId);
            Assert.NotNull(invitation.ExpireTime);
            Assert.Equal(
                expiretime.ToLocalTime().ToString(), 
                invitation.ExpireTime.Value.ToLocalTime().ToString());
            Assert.Equal(user, invitation.UserId);
        }
        
        [Fact]
        public async Task Fail_NoPermissionsException()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server1;
            DateTime expiretime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            Guid user = Ids.UserBId;
            long oldCount = await Context.Invitations.CountAsync(i => true);
            
            SetAuthorizedUserId(user);
            
            CreateInvitationRequest request = new()
            {
                ServerId = serverId,
                IncludeUser = true,
                ExpireTime = expiretime
            };
            CreateInvitationRequestHandler handler = new(Context, UserProvider);
            
            //Act
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Assert.Equal(oldCount, await Context.Invitations.CountAsync(s => true));
        }
    }
}