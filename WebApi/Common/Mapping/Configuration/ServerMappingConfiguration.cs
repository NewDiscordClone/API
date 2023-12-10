using Mapster;
using Sparkle.Domain;
using Sparkle.Application.Servers.Commands.UpdateServer;
using Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileRoles;
using Sparkle.Contracts.Servers;

namespace Sparkle.WebApi.Common.Mapping.Configuration
{
    public class ServerMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(UpdateServerRequest Request, string ServerId), UpdateServerCommand>()
                .Map(dest => dest.ServerId, src => src.ServerId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<(Guid ProfileId, UpdateServerProfileRolesRequest Request), UpdateServerProfileRolesCommand>()
                .Map(dest => dest.Roles, src => src.Request.Roles)
                .Map(dest => dest.ProfileId, src => src.ProfileId);

            config.NewConfig<ServerProfile, ServerProfileLookupResponse>()
                .Map(dest => dest.Name, src => src.DisplayName)
                .Map(dest => dest.MainRole, src => src.Roles.MaxBy(role => role.Priority));

            config.NewConfig<ServerProfile, ServerProfileResponse>()
                .Map(dest => dest.Name, src => src.DisplayName);
        }
    }
}
