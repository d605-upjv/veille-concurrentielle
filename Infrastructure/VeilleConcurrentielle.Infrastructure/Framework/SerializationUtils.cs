using System.Text.Json;

namespace VeilleConcurrentielle.Infrastructure.Framework
{
    public static class SerializationUtils
    {
        public static string Serialize(object obj)
        {
            // TODO: Use binary serializer for better performance
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
        public static T Deserialize<T>(string serialized)
        {
            // TODO: Use binary serializer for better performance
            return JsonSerializer.Deserialize<T>(serialized, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
