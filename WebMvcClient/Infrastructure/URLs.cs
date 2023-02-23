using WebMvcClient.Models;

namespace WebMvcClient.Infrastructure
{
    public static class URLs
    {
        public static class Events
        {
            public static string GetEventByIdUrl(string hostUrl, int eventId) => $"{hostUrl}/events/detailed/?eventId={eventId}";
            public static string GetAllEventsUrl(string hostUrl) => $"{hostUrl}/events/all";
            public static string PostEventUrl(string hostUrl) => $"{hostUrl}/events/add";
            public static string UpdateEventUrl(string hostUrl) => $"{hostUrl}/events/edit";
            public static string DeleteEventUrl(string hostUrl, int eventId) => $"{hostUrl}/event/delete/?eventId={eventId}";
        }
    }
}
