using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Commands.JoinServer;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Servers.Commands
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
            Invitation invitation = await Context.Invitations.FindAsync(invitationId);
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            int oldCount = server.Profiles.Count;

            SetAuthorizedUserId(userId);

            JoinServerCommand request = new()
            {
                InvitationId = invitationId
            };

            Mock<IServerProfileRepository> repository = new();

            JoinServerCommandHandler handler = new(Context, UserProvider, Mapper, repository.Object);

            //Act 
            await handler.Handle(request, CancellationToken);

            //Assert
            server = await Context.Servers.FindAsync(invitation.ServerId);
            Assert.Equal(oldCount + 1, server.Profiles.Count);
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

            JoinServerCommand request = new()
            {
                InvitationId = invitationId
            };
            JoinServerCommandHandler handler = new(Context, UserProvider, Mapper);

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

            JoinServerCommand request = new()
            {
                InvitationId = invitationId
            };
            JoinServerCommandHandler handler = new(Context, UserProvider, Mapper);

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

            JoinServerCommand request = new()
            {
                InvitationId = invitationId
            };
            JoinServerCommandHandler handler = new(Context, UserProvider, Mapper);

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