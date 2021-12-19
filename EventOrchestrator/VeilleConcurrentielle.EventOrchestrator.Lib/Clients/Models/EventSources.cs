using System.Runtime.Serialization;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models
{
    public enum EventSources
    {
        [EnumMember(Value = "Test")]
        Test
    }
}
