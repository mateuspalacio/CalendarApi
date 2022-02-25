using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Calendar.Domain.Exceptions
{
    public class EmailNotFoundException : ErrorResponseException
    {
        public EmailNotFoundException(ErrorResponse errorResponse) : base(errorResponse)
        {

        }
    }
}
