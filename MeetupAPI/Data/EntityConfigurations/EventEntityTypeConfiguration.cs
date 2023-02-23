using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetupAPI.Data.EntityConfigurations
{
    public class EventEntityTypeConfiguration
        : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable(nameof(Event));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired(false);

            builder.Property(x => x.Plan)
                .IsRequired(false);

            builder.Property(x => x.Organizer)
                .IsRequired();

            builder.Property(x => x.Speaker)
                .IsRequired();

            builder.Property(x => x.Location)
                .IsRequired();

            builder.Property(x => x.DateTime)
                .IsRequired();
        }
    }
}