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
        public Task<Events> GetEventsForAccount();
        public Task<Event> CreateEventsForAccount(EventRequest e);
    }
}
