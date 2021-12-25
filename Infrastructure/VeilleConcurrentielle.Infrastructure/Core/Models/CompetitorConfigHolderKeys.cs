using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum CompetitorConfigHolderKeys
    {
        [EnumMember(Value = "UniqueId")]
        UniqueId,
        [EnumMember(Value = "EAN")]
        EAN,
        [EnumMember(Value = "ProfileUrl")]
        ProfileUrl
    }
}
