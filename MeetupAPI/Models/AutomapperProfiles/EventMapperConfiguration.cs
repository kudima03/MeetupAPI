using AutoMapper;
using MeetupAPI.Models.DTOs;

namespace MeetupAPI.Models.AutomapperProfiles
{
    public class EventMapperConfiguration : Profile
    {
        public EventMapperConfiguration()
        {
            CreateMap<Event, EventDTO>();
            CreateMap<Event, EventDetailedDTO>();
            CreateMap<EventDetailedDTO, Event>();
            CreateMap<EventDTO, Event>();
        }
    }
}
