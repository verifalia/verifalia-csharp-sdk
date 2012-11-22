using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// Signals an issue with the credentials provided to the Verifalia service.
    /// </summary>
    public class AuthorizationException : VerifaliaException
    {
        public AuthorizationException(string message)
            : base(message)
        {
        }
    }
}
