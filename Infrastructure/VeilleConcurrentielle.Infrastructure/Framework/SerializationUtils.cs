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
        public static T Deserialize<T>(string serialized) where T : class
        {
            // TODO: Use binary serializer for better performance
            return Deserialize(serialized, typeof(T)) as T;
        }
        public static object Deserialize(string serialized, Type type)
        {
            return JsonSerializer.Deserialize(serialized, type, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
