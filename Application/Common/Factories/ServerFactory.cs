using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Factories
{

    public class ServerFactory
    {
        private readonly IRoleFactory _roleFactory;

        private readonly IAuthorizedUserProvider _userProvider;
        public ServerFactory(IRoleFactory roleFactory, IAuthorizedUserProvider userProvider)
        {
            _roleFactory = roleFactory;
            _userProvider = userProvider;
        }

        private (Server Server, ServerProfile Owner) BaseServer(string title, string? image)
        {
            List<Role> roles = _roleFactory.GetDefaultServerRoles();
            (Role ownerRole, Role memberRole) = (roles[0], roles[1]);

            Server server = new()
            {
                Title = title,
                Image = image,
                Roles = roles.ConvertAll(role => role.Id),
            };

            ServerProfile owner = new()
            {
                UserId = _userProvider.GetUserId(),
                ServerId = server.Id,
                Roles = { ownerRole }
            };

            server.Profiles.Add(owner.Id);

            return (server, owner);
        }

        public (Server Server, ServerProfile Owner, Channel Channel) DefaultServer(string title, string? image)
        {
            (Server server, ServerProfile owner) = BaseServer(title, image);

            Channel welcomeChannel = new()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                ServerId = server.Id,
                Profiles = { owner.Id },
                Title = Constants.Constants.Channel.WelcomeChannelName
            };

            return (server, owner, welcomeChannel);
        }
    }
}
