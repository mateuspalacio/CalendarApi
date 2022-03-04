using AutoMapper;
using Calendar.Domain.Models.DTO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Calendar.Domain.Exceptions;

namespace Calendar.Domain.Services.ServicesImpl
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly Credentials.Credentials _credentials;
        public IMapper Mapper { get; }
        private CalendarService _service;

        public GoogleCalendarService(Credentials.Credentials credentials, IMapper mapper)
        {
            _credentials = credentials;
            Mapper = mapper;
            _service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials.GetCredentials().Result,
                ApplicationName = _credentials.ApplicationName,
            });
        }
        

        public async Task<Event> CreateEventsForAccount(EventRequest e)
        {
            // Create Google Calendar API service.
            // here we check if a property is null
            var result = typeof(EventRequest).GetProperties()
                  .Select(x => new { property = x.Name, value = x.GetValue(e) })
                  .Where(x => x.value == null)
                  .ToList();
            if (result.Count > 1)
                throw new Exceptions.MissingFieldException(new ErrorResponse() { Message = $"The following properties are missing and are mandatory: {string.Join(", ", result.Select(p => p.property).ToList())}", StatusCode = (int)HttpStatusCode.BadRequest });
            else if (result.Count == 1)
                throw new Exceptions.MissingFieldException(new ErrorResponse() { Message = $"The following property is missing and is mandatory {result.First().property}", StatusCode = (int)HttpStatusCode.BadRequest });
            Event eventCreate = Mapper.Map<Event>(e);
           
            EventsResource.InsertRequest request = _service.Events.Insert(eventCreate, "primary");

            var events = request.Execute();
            return events;
        }

        public async Task<Events> GetEventsForAccount(int? next)
        {
            // Create Google Calendar API service.
            // Define parameters of request.
            EventsResource.ListRequest request = _service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            if(next is not null)
                request.MaxResults = next.Value;
            else
                request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                return events;
            }
            else
            {
                throw new NoUpcomingEventsException(new ErrorResponse()
                    {
                        Message = "No Upcoming Events for this Account",
                        StatusCode = (int)HttpStatusCode.NotFound
                    }
                ); 
            }
        }

        public async Task<Events> GetEventsForAccountTimePeriod(DateTime minDateTime, DateTime maxDateTime)
        {
            // Define parameters of request.
            EventsResource.ListRequest request = _service.Events.List("primary");
            request.TimeMin = minDateTime;
            request.TimeMax = maxDateTime;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                return events;
            }
            else
            {
                throw new NoUpcomingEventsException(new ErrorResponse()
                {
                    Message = "No Upcoming Events for this Account",
                    StatusCode = (int)HttpStatusCode.NotFound
                }
                );
            }
        }

        public async Task<Event> GetEventById(string eventId)
        {
            EventsResource.GetRequest request = _service.Events.Get("primary", eventId);

            // List event.
            Event events = request.Execute();
            if (events is null)
                throw new EventNotFoundException(new ErrorResponse() { Message = $"No such event for id {eventId} on the primary calendar", StatusCode = (int)HttpStatusCode.NotFound });

            return events;
        }
        public async Task<Event> UpdateEventsForAccount(string eventId, EventRequest request)
        {
            
            // here we check if a property is null
            var result = typeof(EventRequest).GetProperties()
                  .Select(x => new { property = x.Name, value = x.GetValue(request) })
                  .Where(x => x.value == null)
                  .ToList();
            if (result.Count > 1)
                throw new Exceptions.MissingFieldException(new ErrorResponse() { Message = $"The following properties are missing and are mandatory: {string.Join(", ", result.Select(p => p.property).ToList())}", StatusCode = (int)HttpStatusCode.BadRequest });
            else if (result.Count == 1)
                throw new Exceptions.MissingFieldException(new ErrorResponse() { Message = $"The following property is missing and is mandatory {result.First().property}", StatusCode = (int)HttpStatusCode.BadRequest });
            Event eventUpdate = Mapper.Map<Event>(request);
            // Checking if event exists. If it doesn't, then this method will call NotFoundException
            await GetEventById(eventId);

            EventsResource.UpdateRequest req = _service.Events.Update(eventUpdate, "primary", eventId);

            var events = req.Execute();
            return events;
        }

        public async Task DeleteEventsForAccount(string eventId)
        {
            EventsResource.GetRequest checkIfExists = _service.Events.Get("primary", eventId);
            if (checkIfExists is null)
                throw new EventNotFoundException(new ErrorResponse() { Message = $"No such event for id {eventId} on the primary calendar", StatusCode = (int)HttpStatusCode.NotFound });
            EventsResource.DeleteRequest req = _service.Events.Delete("primary", eventId);

            var events = req.Execute();
            if(events != "")
                throw new InternalErrorException(new ErrorResponse() { Message = $"Unknown Internal Error", StatusCode = (int)HttpStatusCode.InternalServerError });
        }

        public async Task<Events> UpdateRecurringEventsForAccount(string recurringEventId, EventRequest request)
        {
            var result = typeof(EventRequest).GetProperties()
                  .Select(x => new { property = x.Name, value = x.GetValue(request) })
                  .Where(x => x.value == null)
                  .ToList();
            if (result.Count > 1)
                throw new Exceptions.MissingFieldException(new ErrorResponse() { Message = $"The following properties are missing and are mandatory: {string.Join(", ", result.Select(p => p.property).ToList())}", StatusCode = (int)HttpStatusCode.BadRequest });
            else if (result.Count == 1)
                throw new Exceptions.MissingFieldException(new ErrorResponse() { Message = $"The following property is missing and is mandatory {result.First().property}", StatusCode = (int)HttpStatusCode.BadRequest });
            Event eventUpdate = Mapper.Map<Event>(request);
            // Checking if event exists. If it doesn't, then this method will call NotFoundException
            // await GetEventById(eventId);

            EventsResource.InstancesRequest req = _service.Events.Instances("primary", recurringEventId);
            var events = req.Execute();

            foreach (var ev in events.Items)
                await UpdateEventsForAccount(ev.Id, request);

            req = _service.Events.Instances("primary", recurringEventId);
            events = req.Execute();

            return events;
        }
    }
    /* 
     var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials.GetCredentials().Result,
                ApplicationName = _credentials.ApplicationName,
            });
     */
}
