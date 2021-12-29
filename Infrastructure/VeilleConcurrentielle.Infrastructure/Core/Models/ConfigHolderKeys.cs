using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum ConfigHolderKeys
    {
        [EnumMember(Value = "UniqueId")]
        UniqueId,
        [EnumMember(Value = "ProductPageUrl")]
        ProductPageUrl
    }
}
