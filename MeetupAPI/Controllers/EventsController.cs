using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MeetupAPI.Data;
using MeetupAPI.Models;
using MeetupAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace MeetupAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventContext _eventContext;

        private readonly IValidator<EventDetailedDTO> _eventValidator;

        private readonly IMapper _mapper;

        public EventsController(EventContext eventContext, IValidator<EventDetailedDTO> eventValidator, IMapper mapper)
        {
            _eventContext = eventContext;
            _eventValidator = eventValidator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            var query = _eventContext.Events.AsNoTracking().ProjectTo<EventDTO>(_mapper.ConfigurationProvider);
            return Ok(await query.ToListAsync());
        }

        [HttpGet]
        [Route("detailed")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Event), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDetailedDTO>> GetById([FromQuery] int eventId)
        {
            if (eventId <= 0)
            {
                return BadRequest("Id cannot be less than zero.");
            }

            var entity = await _eventContext.Events.AsNoTracking()
                .ProjectTo<EventDetailedDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == eventId);

            if (entity == null)
            {
                return NotFound(new EventDetailedDTO() { Id = eventId });
            }

            return Ok(entity);
        }

        [HttpPost]
        [Route("add")]
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
            await _eventContext.Events.AddAsync(_mapper.Map<Event>(@event));
            await _eventContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("edit")]
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

            if (!await _eventContext.Events.AnyAsync(x => x.Id == @event.Id))
            {
                return NotFound("Event to edit not found.");
            }

            _eventContext.Events.Update(_mapper.Map<Event>(@event));
            await _eventContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] int eventId)
        {
            if (eventId <= 0)
            {
                return BadRequest("Id cannot be less than zero.");
            }

            var entityToDelete = await _eventContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);
            if (entityToDelete == null)
            {
                return NotFound("Event not found.");
            }

            _eventContext.Events.Remove(entityToDelete);
            await _eventContext.SaveChangesAsync();
            return Ok();
        }
    }
}