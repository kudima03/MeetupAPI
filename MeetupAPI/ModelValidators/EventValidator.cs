using FluentValidation;
using MeetupAPI.Models.DTOs;

namespace MeetupAPI.ModelValidators
{
    public class EventValidator : AbstractValidator<EventDetailedDTO>
    {
        public EventValidator()
        {
            RuleFor(x=>x)
                .NotNull()
                .WithMessage("Event cannot be null.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.");

            RuleFor(x => x.Organizer)
                .NotEmpty()
                .WithMessage("Organizer cannot be null or empty.");

            RuleFor(x => x.Speaker)
                .NotEmpty()
                .WithMessage("Speaker cannot be null or empty.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location cannot be null.");

            RuleFor(x => x.DateTime)
                .Must(dateTime => dateTime >= DateTime.Now)
                .WithMessage("DateTime cannot be less than current time.");
        }
    }
}
