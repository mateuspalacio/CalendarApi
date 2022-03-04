using Calendar.Domain.Models.DTO;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Services
{
    public interface IGoogleCalendarService
    {
        public Task<Events> GetEventsForAccount(int? next);
        public Task<Events> GetEventsForAccountTimePeriod(DateTime minDateTime, DateTime maxDateTime);
        public Task<Event> GetEventById(string eventId);
        public Task<Event> CreateEventsForAccount(EventRequest e);
        public Task<Event> UpdateEventsForAccount(string eventId, EventRequest request);
        public Task<Events> UpdateRecurringEventsForAccount(string recurringEventId, EventRequest request);
        public Task DeleteEventsForAccount(string eventId);
    }
}
