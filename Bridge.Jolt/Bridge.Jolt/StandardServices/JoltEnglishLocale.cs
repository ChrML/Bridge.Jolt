using Bridge;
using Jolt.Abstractions;

namespace Jolt.StandardServices.Default
{
    /// <summary>
    /// Implements the locale interface (<see cref="IJoltLocale"/>) with the default English- language strings.
    /// </summary>
    [Reflectable(true)]
    public class JoltEnglishLocale : IJoltLocale
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public string NothingToShow => "Nothing to show.";

        public string UnableToConnect => "Could not connect.";

        /// <inheritdoc/>
        public string GetHttpStatusCodeDescription(int code)
        {
            switch (code)
            {
                case 400: return "Bad request";
                case 401: return "Not authorized";
                case 403: return "Forbidden access";
                case 404: return "The requested resource was not found";
                case 405: return "The request method not allowed";
                case 409: return "Conflict";
                case 415: return "Unsupported media type";
                case 422: return "Unable to process the request";
                case 500: return "Unable to process the request because of an error";
                case 501: return "Feature is not implemented";
                case 502: return "Bad gateway";
                default: return null;
            }
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
