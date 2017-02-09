using System;
using RestSharp;

namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// Base class for exceptions thrown from this assembly.
    /// </summary>
    public class VerifaliaException : ApplicationException
    {
        /// <summary>
        /// Original response sent by Verifalia servers.
        /// </summary>
        public IRestResponse Response { get; set; }

        public VerifaliaException(string message)
            : base(message)
        {
        }

        public VerifaliaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
