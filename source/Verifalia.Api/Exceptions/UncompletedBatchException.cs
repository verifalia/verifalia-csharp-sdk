namespace Verifalia.Api.Exceptions
{
    /// <summary>
    /// An exception thrown in the event the client finishes to poll the API endpoints for the specified maximum duration or iterations
    /// count, without leading to a final result.
    /// </summary>
    public class UncompletedBatchException : VerifaliaException
    {
        public UncompletedBatchException(string message)
            : base(message)
        {
        }
    }
}