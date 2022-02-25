using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Exceptions
{
    public class ErrorResponseException : Exception
    {
        public ErrorResponse ErrorResponse { get; set; }
        public ErrorResponseException(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }
    }
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
