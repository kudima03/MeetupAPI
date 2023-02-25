using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvcClient.Models.DTOs;
using WebMvcClient.Services;
using WebMvcClient.ViewModels;

namespace WebMvcClient.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventsHttpClient _eventsHttpClient;

        private readonly IMapper _mapper;

        private readonly IValidator<EventDetailedViewModel> _validator;

        public EventsController(IEventsHttpClient eventsHttpClient, IMapper mapper, IValidator<EventDetailedViewModel> validator)
        {
            _eventsHttpClient = eventsHttpClient;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Index()
        {
            var events = await _eventsHttpClient.GetAllEventsAsync();
            return View(events.AsQueryable().ProjectTo<EventViewModel>(_mapper.ConfigurationProvider).AsEnumerable());
        }

        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> EventDetails(int eventId)
        {
            return View(_mapper.Map<EventDetailedViewModel>(await _eventsHttpClient.GetEventAsync(eventId)));
        }

        [HttpGet]
        [Route("create")]
        public IActionResult CreateEvent()
        {
            return View("CreateOrUpdateEvent", new EventDetailedViewModel());
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEventz([FromForm] EventDetailedViewModel @event)
        {
            var validationResult = _validator.Validate(@event);
            if (!validationResult.IsValid)
            {
                return View("ValidationErrors", validationResult.Errors);
            }
            await _eventsHttpClient.CreateEventAsync(_mapper.Map<EventDetailedDTO>(@event));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> UpdateEvent(int eventId)
        {
            return View("CreateOrUpdateEvent", _mapper.Map<EventDetailedViewModel>(await _eventsHttpClient.GetEventAsync(eventId)));
        }

        [HttpPost]
        [Route("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent([FromForm] EventDetailedViewModel @event)
        {
            var validationResult = _validator.Validate(@event);
            if (!validationResult.IsValid)
            {
                return View("ValidationErrors", validationResult.Errors);
            }
            await _eventsHttpClient.UpdateEventAsync(_mapper.Map<EventDetailedDTO>(@event));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            await _eventsHttpClient.DeleteEventAsync(eventId);
            return RedirectToAction(nameof(Index));
        }
    }
}