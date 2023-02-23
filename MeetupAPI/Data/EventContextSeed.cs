using MeetupAPI.Models;
using System.Text.Json;

namespace MeetupAPI.Data
{
    public class EventContextSeed
    {
        public async Task SeedAsync(EventContext context, IWebHostEnvironment env, ILogger<EventContextSeed> logger)
        {
            if (!context.Events.Any())
            {
                await context.Events.AddRangeAsync(await GetPreconfiguredEventsFromFileAsync(env, logger));
                await context.SaveChangesAsync();
            }
        }

        private async Task<IEnumerable<Event>> GetPreconfiguredEventsFromFileAsync(IWebHostEnvironment env, ILogger<EventContextSeed> logger)
        {
            var jsonBooksFile = Path.Combine(env.ContentRootPath, "Setup", "Demo events.json");
            if (!File.Exists(jsonBooksFile))
            {
                logger.LogError("Setup file not found in " + jsonBooksFile);
                return new List<Event>();
            }
            try
            {
                return JsonSerializer.Deserialize<List<Event>>(await File.ReadAllTextAsync(jsonBooksFile));
            }
            catch (Exception)
            {
                logger.LogError("Unable to deserialize setup file " + jsonBooksFile);
                return new List<Event>();
            }
        }
    }
}
