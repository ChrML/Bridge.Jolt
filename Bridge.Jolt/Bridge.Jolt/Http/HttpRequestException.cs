using System;

namespace Jolt.Http
{
    /// <summary>
    /// Class for exceptions thrown by HTTP requests.
    /// </summary>
    public class HttpRequestException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <param name="responsePayload"></param>
        public HttpRequestException(string message, int? statusCode, object responsePayload)
            : base(message)
        {
            this.StatusCode = statusCode;
            this.ResponsePayload = responsePayload;
        }

        /// <summary>
        /// Gets the payload that was returned by the server.
        /// </summary>
        public object ResponsePayload { get; }

        /// <summary>
        /// Gets the status code that was returned by the server.
        /// </summary>
        public int? StatusCode { get; }
    }
}
