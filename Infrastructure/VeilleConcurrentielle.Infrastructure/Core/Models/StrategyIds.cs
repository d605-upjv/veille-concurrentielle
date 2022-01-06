using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum StrategyIds
    {
        [EnumMember(Value = "OverallAveragePrice")]
        OverallAveragePrice,
        [EnumMember(Value = "OverallCheaperPrice")]
        OverallCheaperPrice,
        [EnumMember(Value = "FivePercentAboveMeanPrice")]
        FivePercentAboveMeanPrice
    }
}
