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

        public (Server Server, ServerProfile Owner, List<Channel> Channels) DefaultServer(string title, string? image)
        {
            (Server server, ServerProfile owner) = BaseServer(title, image);

            Channel welcomeChannel = CreateChannel(server, owner, Constants.Constants.Channel.WelcomeChannelTitle);
            List<Channel> channels = new() { welcomeChannel };

            return (server, owner, channels);
        }

        private static Channel CreateChannel(Server server, ServerProfile owner, string title)
        {
            return new()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                ServerId = server.Id,
                Profiles = { owner.Id },
                Title = title
            };
        }

        public (Server Server, ServerProfile Owner, List<Channel> Channels) GamingServer(string title, string? image)
        {
            (Server server, ServerProfile owner, List<Channel> channels) = DefaultServer(title, image);

            Channel CreateFunc(string title) => CreateChannel(server, owner, title);

            Channel common = CreateFunc("common");
            Channel lobby = CreateFunc("lobby");
            Channel games = CreateFunc("game");
            Channel bestMoments = CreateFunc("best-moments");

            List<Channel> newChannels = new()
            {
                common, lobby, games, bestMoments
            };
            channels.AddRange(newChannels);

            return (server, owner, channels);
        }

        public (Server Server, ServerProfile Owner, List<Channel> Channels) StudyServer(string title, string? image)
        {
            (Server server, ServerProfile owner, List<Channel> channels) = DefaultServer(title, image);

            Channel CreateFunc(string title) => CreateChannel(server, owner, title);

            Channel common = CreateFunc("common");
            Channel homework = CreateFunc("help-with-homework");

            List<Channel> newChannels = new()
            {
                common, homework
            };
            channels.AddRange(newChannels);

            return (server, owner, channels);
        }
    }
}
