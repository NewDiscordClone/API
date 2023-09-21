using Mapster;
using Sparkle.Application.Invitations.Commands.CreateInvitation;
using Sparkle.Contracts.Invitations;

namespace Sparkle.WebApi.Common.Mapping.Configuration
{
    public class InvitationMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(string ServerId, CreateInvitationRequest Request), CreateInvitationCommand>()
                .Map(dest => dest.ServerId, src => src.ServerId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
