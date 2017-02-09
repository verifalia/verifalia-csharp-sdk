namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// Signals an issue with the credentials provided to the Verifalia service.
    /// </summary>
    public class UncompletedBatchException : VerifaliaException
    {
        public UncompletedBatchException(string message)
            : base(message)
        {
        }
    }
}