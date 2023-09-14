using Mapster;
using Sparkle.Application.Common.Servers.Commands.ChangeServerProfileRoles;
using Sparkle.Application.Common.Servers.Commands.UpdateServer;
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

            config.NewConfig<(string ServerId, Guid UserId, UpdateServerProfileRolesRequest Request), UpdateServerProfileRolesCommand>()
                .Map(dest => dest.Roles, src => src.Request.Roles);
        }
    }
}
