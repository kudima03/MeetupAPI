namespace WebMvcClient.ViewModels
{
    public class EventDetailedViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Plan { get; set; }

        public string Organizer { get; set; } = null!;

        public string Speaker { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime DateTime { get; set; }
    }
}
