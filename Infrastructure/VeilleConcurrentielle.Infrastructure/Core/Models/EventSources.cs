using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum EventSources
    {
        [EnumMember(Value = "Test")]
        Test,
        [EnumMember(Value = "ProdouctService")]
        ProductService,
        [EnumMember(Value = "Scraper")]
        Scraper
    }
}
