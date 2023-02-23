﻿namespace MeetupAPI.Models.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Organizer { get; set; } = null!;

        public string Location { get; set; } = null!;
    }
}
