using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// Base class for exceptions thrown from this assembly.
    /// </summary>
    public class VerifaliaException : ApplicationException
    {
        public IRestResponse Response { get; set; }

        public VerifaliaException(string message)
            : base(message)
        {
        }
    }
}
