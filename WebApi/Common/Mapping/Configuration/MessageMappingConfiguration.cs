using Mapster;
using Sparkle.Application.Messages.Commands.AddMessage;
using Sparkle.Contracts.Messages;

namespace Sparkle.WebApi.Common.Mapping.Configuration
{
    public class MessageMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(AddMessageRequest Request, string ChatId), AddMessageCommand>()
                .Map(dest => dest.ChatId, src => src.ChatId)
                .Map(dest => dest, src => src.Request);
        }
    }
}
