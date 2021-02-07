using System;

namespace Jolt
{
    // NOTE: The System.IServiceProvider would be the correct interface to use for this.
    //       However Bridge.NET has disabled it and does not allow us to redeclare it.

    /// <summary>
    /// Represents a class that can provide services for the application.
    /// </summary>
    public interface IServices
    {
        /// <summary>
        /// Gets the service of the given service type if it's available.
        /// </summary>
        /// <param name="serviceType">The service type to get.</param>
        /// <returns>Returns an instance of the requested service.</returns>
        object GetService(Type serviceType);
    }
}
