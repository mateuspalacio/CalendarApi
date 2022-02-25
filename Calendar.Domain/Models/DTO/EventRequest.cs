using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Models.DTO
{
    public class EventRequest
    {
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }
        public List<string> Recurrence { get; set; }
        public List<EventAttendee>? Attendees { get; set; }
        public RemindersData Reminders { get; set; }

    }

    public class EventDateTime
    {
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; }
        
    }
    public class EventAttendee
    {
        public string Email { get; set; }
    }
    public class RemindersData
    {
        public List<EventReminder> Overrides { get; set; }
        public bool UseDefault { get; init; } = false;
    }
    public class EventReminder
    {
        public int Minutes { get; set; }
        public string Method { get; set; }
    }
}
