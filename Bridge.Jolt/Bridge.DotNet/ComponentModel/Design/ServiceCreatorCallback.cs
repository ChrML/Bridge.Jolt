namespace System.ComponentModel.Design
{
    /// <summary>
    /// Provides a callback mechanism that can create an instance of a service on demand.
    /// </summary>
    /// <param name="container">The service container that requested the creation of the service.</param>
    /// <param name="serviceType">The type of service to create.</param>
    /// <returns>The service specified by serviceType, or <c>null</c> if the service could not be created.</returns>
    public delegate object ServiceCreatorCallback(IServiceContainer container, Type serviceType);
}
