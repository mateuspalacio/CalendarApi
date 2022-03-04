using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Exceptions
{
    public class EventNotFoundException : ErrorResponseException
    {
        public EventNotFoundException(ErrorResponse errorResponse) : base(errorResponse)
        {

        }
    }
}
