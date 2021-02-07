using Bridge;
using Jolt.Abstractions;
using System;

namespace Jolt.Services.Default
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IErrorHandler"/> service.
    /// </summary>
    [Reflectable(true)]
    class DefaultErrorHandler: IErrorHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultErrorHandler"/> class.
        /// </summary>
        public DefaultErrorHandler()
        {
        }

        /// <inheritdoc/>
        public void OnError(Exception exception, string message = null)
        {
            Console.Write(exception.Message + " " + message);
        }
    }
}
