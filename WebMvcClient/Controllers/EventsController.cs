using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public EventsController(IEventsHttpClient eventsHttpClient, IMapper mapper)
        {
            _eventsHttpClient = eventsHttpClient;
            _mapper = mapper;
        }

        [Route("all")]
        public async Task<IActionResult> Index()
        {
            var events = await _eventsHttpClient.GetAllEventsAsync();
            return View(events.AsQueryable().ProjectTo<EventViewModel>(_mapper.ConfigurationProvider));
        }

        [Route("details")]
        public async Task<IActionResult> EventDetails(int eventId)
        {
            return View(await _eventsHttpClient.GetEventAsync(eventId));
        }

        [Route("create")]
        public async Task<IActionResult> CreateEvent(EventDetailedViewModel @event)
        {
            await _eventsHttpClient.CreateEventAsync(_mapper.Map<EventDetailedDTO>(@event));
            return RedirectToAction(nameof(Index));
        }

        [Route("edit")]
        public async Task<IActionResult> UpdateEvent(EventDetailedViewModel @event)
        {
            await _eventsHttpClient.UpdateEventAsync(_mapper.Map<EventDetailedDTO>(@event));
            return RedirectToAction(nameof(Index));
        }
    }
}