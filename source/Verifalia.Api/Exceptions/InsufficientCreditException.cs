﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// Indicates that the credit of the requesting account is not large enough to perform an operation.
    /// </summary>
    public class InsufficientCreditException : VerifaliaException
    {
        public InsufficientCreditException(string message)
            : base(message)
        {
        }
    }
}
