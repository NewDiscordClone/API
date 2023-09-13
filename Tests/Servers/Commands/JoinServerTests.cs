using Application.Commands.Servers.JoinServer;
using Application.Common.Exceptions;
using Application.Models;
using Tests.Common;

namespace Tests.Servers.Commands
{
    public class JoinServerTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            string invitationId = Ids.Invitation1;
            Guid userId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            JoinServerRequest request = new()
            {
                InvitationId = invitationId
            };
            JoinServerRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act 
            await handler.Handle(request, CancellationToken);

            //Assert
            Invitation invitation = await Context.Invitations.FindAsync(invitationId);
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            Assert.Contains(server.ServerProfiles, sp => sp.UserId == userId);
        }
        [Fact]
        public async Task Fail_InvitationIsExpired()
        {
            //Arrange
            CreateDatabase();
            string invitationId = Ids.Invitation3;
            Guid userId = Ids.UserDId;
            long oldCount = await Context.Invitations.CountAsync(s => true);

            SetAuthorizedUserId(userId);

            JoinServerRequest request = new()
            {
                InvitationId = invitationId
            };
            JoinServerRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act 
            //Assert
            Invitation invitation = await Context.Invitations.FindAsync(invitationId);
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await Context.Invitations.FindAsync(invitationId));
            Assert.Equal(oldCount - 1, await Context.Invitations.CountAsync(s => true));
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
        }
        [Fact]
        public async Task Fail_YouAlreadyAServerMember()
        {
            //Arrange
            CreateDatabase();
            string invitationId = Ids.Invitation2;
            Guid userId = Ids.UserBId;

            SetAuthorizedUserId(userId);

            JoinServerRequest request = new()
            {
                InvitationId = invitationId
            };
            JoinServerRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act 
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Invitation invitation = await Context.Invitations.FindAsync(invitationId);
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            Assert.Contains(server.ServerProfiles, sp => sp.UserId == userId);
        }
        [Fact]
        public async Task Fail_YouAreBanned()
        {
            //Arrange
            CreateDatabase();
            string invitationId = Ids.Invitation1;
            Guid userId = Ids.UserDId;

            SetAuthorizedUserId(userId);

            JoinServerRequest request = new()
            {
                InvitationId = invitationId
            };
            JoinServerRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act 
            //Assert
            await Assert.ThrowsAsync<NoPermissionsException>(
                async () => await handler.Handle(request, CancellationToken));
            Invitation invitation = await Context.Invitations.FindAsync(invitationId);
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            Assert.DoesNotContain(server.ServerProfiles, sp => sp.UserId == userId);
        }
    }
}