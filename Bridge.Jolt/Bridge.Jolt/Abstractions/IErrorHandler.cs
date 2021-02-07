using System;

namespace Jolt.Abstractions
{
    /// <summary>
    /// Represents any service that may be invoked when unhandled errors occur.
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// Notifies this error handler about an error that occured.
        /// </summary>
        /// <param name="exception">The exception that occured.</param>
        /// <param name="message">Optional additional message relevant to this error.</param>
        void OnError(Exception exception, string message = null);
    }
}
