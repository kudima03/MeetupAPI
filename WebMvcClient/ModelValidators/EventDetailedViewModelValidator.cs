using FluentValidation;
using WebMvcClient.ViewModels;

namespace WebMvcClient.ModelValidators
{
    public class EventDetailedViewModelValidator : AbstractValidator<EventDetailedViewModel>
    {
        public EventDetailedViewModelValidator()
        {
            RuleFor(x=>x)
                .NotNull()
                .WithMessage("Event cannot be null.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Name cannot be null or empty and less 100 symbols.");

            RuleFor(x => x.Organizer)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Organizer cannot be null or empty and less 100 symbols.");

            RuleFor(x => x.Speaker)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Speaker cannot be null or empty and less 100 symbols.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Location cannot be null and less 100 symbols.");

            RuleFor(x => x.DateTime)
                .Must(dateTime => dateTime >= DateTime.Now)
                .WithMessage("DateTime cannot be less than current time.");
        }
    }
}
