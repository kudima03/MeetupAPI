using WebMvcClient.Models.DTOs;

namespace WebMvcClient.Services
{
    public interface IEventsHttpClient
    {
        Task<List<EventDTO>> GetAllEventsAsync();
        Task<EventDetailedDTO> GetEventAsync(int eventId);
        Task CreateEventAsync(EventDetailedDTO @event);
        Task UpdateEventAsync(EventDetailedDTO @event);
        Task DeleteEventAsync(int eventId);
    }
}
