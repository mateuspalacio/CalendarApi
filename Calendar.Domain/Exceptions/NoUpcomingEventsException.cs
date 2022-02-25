using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Exceptions
{
    public class NoUpcomingEventsException : ErrorResponseException
    {
        public NoUpcomingEventsException(ErrorResponse errorResponse) : base(errorResponse)
        {

        }
    }
}
