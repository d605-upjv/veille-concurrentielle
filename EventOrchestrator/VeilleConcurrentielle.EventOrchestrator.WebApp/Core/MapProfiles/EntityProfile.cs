using AutoMapper;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Core.MapProfiles
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<EventEntity, Event>();
        }
    }
}
