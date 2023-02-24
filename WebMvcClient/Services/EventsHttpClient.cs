using System.Text;
using System.Text.Json;
using WebMvcClient.Infrastructure;
using WebMvcClient.Models.DTOs;

namespace WebMvcClient.Services
{
    public class EventsHttpClient : IEventsHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly string _meetupApiUrl;

        public EventsHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _meetupApiUrl = configuration.GetValue<string>("MeetupApiUrl");
        }

        /// <summary>
        /// Deletes event on apropriate API, specified in config as MeetupApiUrl.<br/>
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task DeleteEventAsync(int eventId)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(URLs.Events.DeleteEventUrl(_meetupApiUrl, eventId)),
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Returns all events from apropriate API, specified in config as MeetupApiUrl.<br/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<List<EventDTO>> GetAllEventsAsync()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLs.Events.GetAllEventsUrl(_meetupApiUrl)),
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EventDTO>>(responseStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EventDTO>();
        }

        /// <summary>
        /// Returns event with <paramref name="eventId"/> from apropriate API, specified in config as MeetupApiUrl.<br/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<EventDetailedDTO> GetEventAsync(int eventId)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLs.Events.GetEventByIdUrl(_meetupApiUrl, eventId)),
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var a = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EventDetailedDTO>(a, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new EventDetailedDTO();
        }

        /// <summary>
        /// Creates event on apropriate API, specified in config as MeetupApiUrl.<br/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task CreateEventAsync(EventDetailedDTO @event)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(URLs.Events.PostEventUrl(_meetupApiUrl)),
                Content = new StringContent(JsonSerializer.Serialize(@event), Encoding.UTF8, "application/json"),
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates event on apropriate API, specified in config as MeetupApiUrl.<br/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task UpdateEventAsync(EventDetailedDTO @event)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(URLs.Events.UpdateEventUrl(_meetupApiUrl)),
                Content = new StringContent(JsonSerializer.Serialize(@event), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
