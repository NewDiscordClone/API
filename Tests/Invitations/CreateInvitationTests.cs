using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Invitations.Commands.CreateInvitation;
using Sparkle.Application.Models;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Invitations
{
    public class CreateInvitationTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string serverId = Ids.Server1;
            DateTime expiretime = DateTime.Now.AddDays(1);
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
            DateTime expiretime = DateTime.Now.AddDays(1);
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