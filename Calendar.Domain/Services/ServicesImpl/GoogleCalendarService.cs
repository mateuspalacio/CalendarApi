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

        public GoogleCalendarService(Credentials.Credentials credentials, IMapper mapper)
        {
            _credentials = credentials;
            Mapper = mapper;
        }
        public async Task<Event> CreateEventsForAccount(EventRequest e)
        {
            // TODO: Figure out "Recurrence"
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials.GetCredentials().Result,
                ApplicationName = _credentials.ApplicationName,
            });
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

           
            //     new Event()
            //{
            //    Summary = "Test summary",
            //    Location = "Rua Paula Ney, 925, Apto 302, Fortaleza - CE",
            //    Description = "Description for test",
            //    Start = new EventDateTime() { DateTime = new DateTime(2022, 02, 25, 10, 30, 00), TimeZone = "America/Belem" },
            //    End = new EventDateTime() { DateTime = new DateTime(2022, 02, 25, 11, 30, 00), TimeZone = "America/Belem" }, // get list of timezones
            //    Recurrence = new List<string>() { "RRULE:FREQ=WEEKLY;BYDAY=FR,SA,MO;COUNT=1" },
            //    Attendees = new List<EventAttendee>() { new EventAttendee() {Email = "mateus_palacio@atlantico.com.br"} }, // consider no email
            //    Reminders = new Event.RemindersData() { Overrides = new List<EventReminder>() { new EventReminder() { Minutes = 10, Method = "popup" } }, UseDefault = false } // consider usedefault
            //};
            EventsResource.InsertRequest request = service.Events.Insert(eventCreate, "primary");

            var events = request.Execute();
            return events;
        }

        public async Task<Events> GetEventsForAccount(int? next)
        {
            // TODO: Create method to receive date and return events for said date
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials.GetCredentials().Result,
                ApplicationName = _credentials.ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
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
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credentials.GetCredentials().Result,
                ApplicationName = _credentials.ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
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

        public Task<Event> UpdateEventsForAccount(string eventId, EventRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteEventsForAccount(string eventId)
        {
            throw new NotImplementedException();
        }
        // TODO Method to delete event or update
    }
}
