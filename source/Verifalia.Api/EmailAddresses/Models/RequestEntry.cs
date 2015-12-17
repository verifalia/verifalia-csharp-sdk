using System;

namespace Verifalia.Api.EmailAddresses.Models
{
    /// <summary>
    /// Represents a single email address entry to be validated.
    /// </summary>
    public class RequestEntry
    {
        private string _custom;

        /// <summary>
        /// The input string to validate, which should represent an email address.
        /// </summary>
        public string InputData { get; private set; }

        /// <summary>
        /// A custom, optional string which is passed back upon completing the validation.
        /// </summary>
        public string Custom
        {
            get { return _custom; }
            set
            {
                if (value != null)
                {
                    if (value.Length > 50)
                    {
                        throw new ArgumentOutOfRangeException("value", String.Format("Custom value '{0}' exceeds the maximum allowed length of 50 characters.", value));
                    }
                }

                _custom = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestEntry"/> class.
        /// </summary>
        /// <param name="inputData">The input data string (email address) to be validated.</param>
        /// <exception cref="System.ArgumentNullException">inputData</exception>
        public RequestEntry(string inputData)
            : this(inputData, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestEntry"/> class.
        /// </summary>
        /// <param name="inputData">The input data string (email address) to be validated.</param>
        /// <param name="custom">A custom, optional string which is passed back upon completing the validation.</param>
        /// <exception cref="System.ArgumentNullException">inputData</exception>
        public RequestEntry(string inputData, string custom)
        {
            if (inputData == null) throw new ArgumentNullException("inputData");
            
            InputData = inputData;
            Custom = custom;
        }
    }
}