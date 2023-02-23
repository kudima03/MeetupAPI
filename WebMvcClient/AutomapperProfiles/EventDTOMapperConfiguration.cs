using AutoMapper;
using WebMvcClient.Models.DTOs;
using WebMvcClient.ViewModels;

namespace WebMvcClient.AutomapperProfiles
{
    public class EventDTOMapperConfiguration : Profile
    {
        public EventDTOMapperConfiguration()
        {
            CreateMap<EventDTO, EventViewModel>();
            CreateMap<EventDetailedDTO, EventDetailedViewModel>();
            CreateMap<EventDetailedViewModel, EventDetailedDTO>();
            CreateMap<EventViewModel, EventDTO>();
        }
    }
}
