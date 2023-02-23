using MeetupAPI.Data.EntityConfigurations;
using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Data
{
    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public EventContext(DbContextOptions<EventContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}