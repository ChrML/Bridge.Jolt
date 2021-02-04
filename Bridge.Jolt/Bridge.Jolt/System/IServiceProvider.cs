using Bridge.Jolt.Services;
using System;

namespace Bridge.Jolt
{
    /// <summary>
    /// Represents a class that can provide services for the application.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Gets the service of the given service type if it's available.
        /// </summary>
        /// <param name="serviceType">The service type to get.</param>
        /// <returns>Returns an instance of the requested service.</returns>
        object GetService(Type serviceType);
    }
}
