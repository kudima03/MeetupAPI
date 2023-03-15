using AutoMapper;
using FluentValidation;
using MeetupAPI.Data.Interfaces;
using MeetupAPI.Models;
using MeetupAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace MeetupAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IAsyncRepository<Event> _eventContext;

        private readonly IValidator<EventDetailedDTO> _eventValidator;

        private readonly IMapper _mapper;

        public EventsController(IAsyncRepository<Event> eventContext, IValidator<EventDetailedDTO> eventValidator, IMapper mapper)
        {
            _eventContext = eventContext;
            _eventValidator = eventValidator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            var events = await _eventContext.GetAllAsync();
            var eventsDTO = from item in events select _mapper.Map<EventDTO>(item);
            return Ok(eventsDTO);
        }

        [HttpGet("{eventId:int}")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Event), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDetailedDTO>> GetById(int eventId)
        {
            if (eventId <= 0)
            {
                return BadRequest("Id cannot be less than zero.");
            }

            var @event = await _eventContext.GetByIdAsync(eventId);

            if (@event == null)
            {
                return NotFound($"Event with id:{eventId} not found.");
            }

            return Ok(_mapper.Map<EventDetailedDTO>(@event));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] EventDetailedDTO @event)
        {
            var validationResult = _eventValidator.Validate(@event);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            @event.Id = 0;
            await _eventContext.CreateAsync(_mapper.Map<Event>(@event));
            await _eventContext.SaveAsync();
            return Ok();
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] EventDetailedDTO @event)
        {
            var validationResult = _eventValidator.Validate(@event);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            var entity = _eventContext.GetById(@event.Id);

            if (entity == null)
            {
                @event.Id = 0;
                await _eventContext.CreateAsync(_mapper.Map<Event>(@event));
                return Ok();
            }
            await _eventContext.UpdateAsync(_mapper.Map<Event>(@event));
            await _eventContext.SaveAsync();
            return Ok();
        }

        [HttpDelete("{eventId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int eventId)
        {
            if (eventId <= 0)
            {
                return BadRequest("Id cannot be less than zero.");
            }

            var entityToDelete = await _eventContext.GetByIdAsync(eventId);
            if (entityToDelete == null)
            {
                return NotFound("Event not found.");
            }

            await _eventContext.DeleteAsync(entityToDelete);
            await _eventContext.SaveAsync();
            return Ok();
        }
    }
}