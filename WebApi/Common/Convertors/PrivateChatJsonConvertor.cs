using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Common.Convertors
{
    public class PrivateChatLookUpConverter : JsonConverter<PrivateChatLookUp>
    {
        public override PrivateChatLookUp? ReadJson(JsonReader reader, Type objectType, PrivateChatLookUp? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            if (jObject["membersCount"] != null)
            {
                return jObject.ToObject<GroupChatLookup>(serializer);
            }
            else
            {
                return jObject.ToObject<PersonalChatLookup>(serializer);
            }
        }

        public override void WriteJson(JsonWriter writer, PrivateChatLookUp? value, JsonSerializer serializer)
        {
            JObject jObject;
            switch (value)
            {
                case GroupChatLookup groupChat:
                    jObject = JObject.FromObject(groupChat, serializer);
                    jObject.WriteTo(writer);
                    break;

                case PersonalChatLookup personalChat:
                    jObject = JObject.FromObject(personalChat, serializer);
                    jObject.WriteTo(writer);
                    break;

                default:
                    throw new ArgumentException("the given value is not a private chat look up");
            }

        }
    }
}