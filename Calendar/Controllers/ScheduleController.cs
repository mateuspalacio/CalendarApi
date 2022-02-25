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
        // TODO: ProducesResponseType, ExceptionHandling
        [HttpGet("detailed")]
        public async Task<Events> GetEvents()
        {
           var result = await _service.GetEventsForAccount();
           return result;
        }

        [HttpPost("create")]
        public async Task<Event> AddEvents([FromBody]EventRequest e)
        {
            var responseCode = Response.StatusCode;
            try
            {
                var result = await _service.CreateEventsForAccount(e);
                return result;
            }
            catch (Domain.Exceptions.MissingFieldException ex) {
                responseCode = (int)HttpStatusCode.BadRequest;
                throw ex.Message;
            }
        }
    }
}