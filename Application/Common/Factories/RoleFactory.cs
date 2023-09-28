using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Factories
{
    public class RoleFactory : IRoleFactory
    {
        // это не финальные наборы клеймом, а наборы клеймом для тестов

        private readonly IRoleRepository _roleRepository;

        public RoleFactory(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        //TODO: Add more claims
        private readonly string[] _serverMemberDefaultClaims
            = { ServerClaims.ChangeServerName };

        //TODO: Add more claims
        private readonly string[] _serverOwnerDefaultClaims =
        {
            ServerClaims.ChangeServerName,
            ServerClaims.ManageServer,
            ServerClaims.ManageRoles,
            ServerClaims.ManageChannels
        };
        //TODO: Add more claims

        private readonly string[] _personalChatMemberClaims =
        {
            ServerClaims.ManageRoles,
            ServerClaims.ManageServer,
            ServerClaims.ManageMessages,
            ServerClaims.ManageChannels
        };

        private readonly string[] _groupChatMemberClaims =
        {
            ServerClaims.ManageRoles,
            ServerClaims.ManageServer,
            ServerClaims.ManageMessages,
            ServerClaims.ManageChannels
        };

        private readonly string[] _groupChatOwnerClaims =
        {
            ServerClaims.ManageMessages,
        };

        private async Task<List<ServerClaim>> CreateClaimsForRoleAsync(Role role, params string[] claims)
        {
            if (!claims.All(claim => ServerClaims.GetClaims().Contains(claim)))
                throw new ArgumentException("Invalid claim(s) provided");

            List<ServerClaim> serverClaims = new();

            foreach (string claim in claims)
            {
                ServerClaim serverClaim = new()
                {
                    ClaimType = claim,
                    RoleId = role.Id
                };
                serverClaims.Add(serverClaim);
            }

            await _roleRepository.AddClaimsToRoleAsync(role, serverClaims);

            return serverClaims;
        }

        private readonly Role _groupChatOwnerRole = new()
        {
            Id = Constants.Constants.Roles.GroupChatOwnerId,
            Name = Constants.Constants.Roles.GroupChatOwnerName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 1,
        };

        private readonly Role _groupChatMemberRole = new()
        {
            Id = Constants.Constants.Roles.GroupChatMemberId,
            Name = Constants.Constants.Roles.GroupChatMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 0,
        };

        private readonly Role _personalChatMemberRole = new()
        {
            Id = Constants.Constants.Roles.PrivateChatMemberId,
            Name = Constants.Constants.Roles.PrivateChatMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 0,
        };

        private readonly Role _serverOwnerRole = new()
        {
            Id = Constants.Constants.Roles.ServerOwnerId,
            Name = Constants.Constants.Roles.ServerOwnerName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 100,
        };

        private readonly Role _serverMemberRole = new()
        {
            Id = Constants.Constants.Roles.ServerMemberId,
            Name = Constants.Constants.Roles.ServerMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 0,
        };
        public Role PersonalChatMemberRole => _personalChatMemberRole;
        public Role GroupChatMemberRole => _groupChatMemberRole;
        public Role ServerOwnerRole => _serverOwnerRole;
        public Role ServerMemberRole => _serverMemberRole;
        public Role GroupChatOwnerRole => _groupChatOwnerRole;

        public string[] GroupChatOwnerClaims => _groupChatOwnerClaims;
        public string[] GroupChatMemberClaims => _groupChatMemberClaims;
        public string[] PersonalChatMemberClaims => _personalChatMemberClaims;
        public string[] ServerOwnerDefaultClaims => _serverOwnerDefaultClaims;
        public string[] ServerMemberDefaultClaims => _serverMemberDefaultClaims;

        public List<Role> GetDefaultServerRoles()
            => new() { ServerOwnerRole, ServerMemberRole };

        public List<Role> GetGroupChatRoles()
            => new() { GroupChatOwnerRole, GroupChatMemberRole };

        public async Task<Role> CreateRoleAsync(string name, string color, int priority, string[] claims, string? serverId)
        {
            Role role = new()
            {
                Name = name,
                Color = color,
                Priority = priority,
                ServerId = serverId,
            };

            await _roleRepository.AddAsync(role);

            await CreateClaimsForRoleAsync(role, claims);

            return role;
        }
    }
}
