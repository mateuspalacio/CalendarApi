﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Exceptions
{
    public class MissingFieldException : ErrorResponseException
    {
        public MissingFieldException(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
