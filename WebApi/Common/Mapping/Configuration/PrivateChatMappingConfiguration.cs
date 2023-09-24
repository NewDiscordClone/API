using Mapster;
using Sparkle.Application.GroupChats.Commands.RemoveUserFromGroupChat;
using Sparkle.Contracts.PrivateChats;

namespace Sparkle.WebApi.Common.Mapping.Configuration
{
    public class PrivateChatMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(RemoveUserFromGroupChatRequest Request, string ChatId), RemoveUserFromGroupChatCommand>()
                .Map(dest => dest.ChatId, src => src.ChatId)
                .Map(dest => dest, src => src.Request);
        }
    }

}
