using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Aggregator.Lib.Contracts
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
