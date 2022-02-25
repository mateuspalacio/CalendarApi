using Calendar.Domain.Models.DTO;
using Calendar.Domain.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Calendar.Controllers
{
    [ApiController]
    [Route("schedule/")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IGoogleCalendarService _service;
        public ScheduleController(ILogger<ScheduleController> logger, IGoogleCalendarService service)
        {
            _logger = logger;
            _service = service;
        }
        // TODO: ProducesResponseType, Put Credentials on a Vault, CI/CD

        /// <summary>
        /// Checks Google Calendar for next x events for the account. Has a default value of 10
        /// </summary>
        /// <param name="next"></param>
        /// <returns>The events</returns>
        /// <response code="200">The events</response>
        /// <response code="404">There are no upcoming events for this account</response>
        [HttpGet("detailed")]
        [ProducesResponseType(typeof(Events), 200)]
        [ProducesResponseType(typeof(Domain.Exceptions.ErrorResponse), 404)]
        public async Task<Events> GetEvents([FromQuery] int? next) // Maybe change to List<Event>?
        {
            var result = new Events();
            if (next is not null)
                  result = await _service.GetEventsForAccount(next);
            else
                  result = await _service.GetEventsForAccount(10);
            return result;
        }

        /// <summary>
        /// Checks Google Calendar for events during a period of time
        /// </summary>
        /// <param name="next"></param>
        /// <returns>The events</returns>
        /// <response code="200">The events</response>
        /// <response code="404">There are no upcoming events for this account</response>
        [HttpGet("detailed/datePeriod")]
        [ProducesResponseType(typeof(Events), 200)]
        [ProducesResponseType(typeof(Domain.Exceptions.ErrorResponse), 404)]
        public async Task<Events> GetEventsForDatePeriod([FromBody] DateSpamRequest dro)
        {
            var result = await _service.GetEventsForAccountTimePeriod(dro.MinDate, dro.MaxDate);
            return result;
        }

        /// <summary>
        /// Adds event to Google Calendar
        /// </summary>
        /// <param name="e"></param>
        /// <returns>The event that was created</returns>
        /// <response code="200">The event</response>
        /// <response code="400">A parameter was missing</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(Events), 200)]
        [ProducesResponseType(typeof(Domain.Exceptions.ErrorResponse), 400)]
        public async Task<Event> AddEvents([FromBody] EventRequest e)
        {
            var result = await _service.CreateEventsForAccount(e);
            return result;
        }

        /// <summary>
        /// Updates event on Google Calendar
        /// </summary>
        /// <param name="e"></param>
        /// <returns>The event that was updated</returns>
        /// <response code="201">The event updated</response>
        /// <response code="400">A parameter was missing</response>
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(Events), 200)]
        [ProducesResponseType(typeof(Domain.Exceptions.ErrorResponse), 400)]
        public async Task<IActionResult> UpdateEvents([FromRoute] string id, [FromBody] EventRequest e)
        {
            var result = await _service.UpdateEventsForAccount(id, e);
            return Ok(result);
        }

        /// <summary>
        /// Remove event from Google Calendar
        /// </summary>
        /// <param name="eventId"></param>
        /// <response code="204">The event was deleted</response>
        /// <response code="404">Event not found</response>
        [HttpPost("delete/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Domain.Exceptions.ErrorResponse), 404)]
        public async Task<IActionResult> RemoveEvents([FromRoute] string eventId)
        {
            await _service.DeleteEventsForAccount(eventId);
            return NoContent();
        }
    }
}