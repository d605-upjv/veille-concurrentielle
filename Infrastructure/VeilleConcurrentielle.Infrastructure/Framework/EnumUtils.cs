using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VeilleConcurrentielle.Infrastructure.Framework
{
    public static class EnumUtils
    {
        public static T GetValueFromString<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
