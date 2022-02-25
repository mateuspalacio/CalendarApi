using AutoMapper;
using Calendar.Domain.Models.DTO;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Event, EventRequest>().ReverseMap();
            CreateMap<Models.DTO.EventDateTime, Google.Apis.Calendar.v3.Data.EventDateTime>().ReverseMap();
            CreateMap<Models.DTO.EventAttendee, Google.Apis.Calendar.v3.Data.EventAttendee>().ReverseMap();
            CreateMap<RemindersData, Event.RemindersData>().ReverseMap();
            CreateMap<Models.DTO.EventReminder, Google.Apis.Calendar.v3.Data.EventReminder>().ReverseMap();
        }
    }
}
