using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Models.DTO
{
    public class DateSpanRequest
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }
}
