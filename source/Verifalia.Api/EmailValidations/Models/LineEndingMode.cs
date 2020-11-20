using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Verifalia.Api.EmailValidations.Models
{
    /// <summary>
    /// The line-ending mode for an input text file provided to the Verifalia API for verification.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LineEndingMode
    {
        /// <summary>
        /// Automatic line-ending detection, attempts to guess the correct line ending from the first chunk of data.
        /// </summary>
        Auto,

        /// <summary>
        /// CR + LF sequence (\r\n), commonly used in files generated on Windows.
        /// </summary>
        CrLf,

        /// <summary>
        /// CR sequence (\r), commonly used in files generated on classic MacOS.
        /// </summary>
        Cr,

        /// <summary>
        /// LF (\n), commonly used in files generated on Unix and Unix-like systems (including Linux and MacOS).
        /// </summary>
        Lf
    }
}