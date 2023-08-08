using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application
{
    public class NullToUndefinedConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteUndefined();
            }
            else
            {
                JToken.FromObject(value).WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}