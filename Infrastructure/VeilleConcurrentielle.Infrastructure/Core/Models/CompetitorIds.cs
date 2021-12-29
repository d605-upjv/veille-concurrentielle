using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum CompetitorIds
    {
        [EnumMember(Value = "ShopA")]
        ShopA,
        [EnumMember(Value = "ShopB")]
        ShopB,
        [EnumMember(Value = "ShopC")]
        ShopC
    }
}
