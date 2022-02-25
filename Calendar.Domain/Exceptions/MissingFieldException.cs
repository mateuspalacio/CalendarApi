using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Exceptions
{
    public class MissingFieldException : Exception
    {
        public MissingFieldException()
        {

        }
        public MissingFieldException(string message)
        : base(message)
        {
        }

        public MissingFieldException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
