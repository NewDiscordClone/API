using Sparkle.Application.Models.LookUps;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sparkle.Application.Common.Convertors
{
    public class PrivateChatLookUpConverter : JsonConverter<PrivateChatLookUp>
    {
        public override PrivateChatLookUp? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            JsonElement element = document.RootElement;

            if (element.TryGetProperty("membersCount", out JsonElement typeElement))
            {
                int? type = typeElement.GetInt32();
                return type switch
                {
                    null => JsonSerializer.Deserialize<PersonalChatLookup>(element.GetRawText(), options),
                    int => JsonSerializer.Deserialize<GroupChatLookup>(element.GetRawText(), options),
                };
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, PrivateChatLookUp value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case PersonalChatLookup privateChat:
                    JsonSerializer.Serialize(writer, privateChat, options);
                    break;
                case GroupChatLookup groupChat:
                    JsonSerializer.Serialize(writer, groupChat, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}