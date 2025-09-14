using Confluent.Kafka;

namespace Playground.Core.Kafka
{
    public class ObjectJsonSerializer : ISerializer<object>
    {
        public byte[] Serialize(object data, SerializationContext context)
        {
            if (data is null) return null;
            if (data is string s) return System.Text.Encoding.UTF8.GetBytes(s);
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}
