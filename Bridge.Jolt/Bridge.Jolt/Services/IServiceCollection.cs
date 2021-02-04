namespace Bridge.Jolt.Services
{
    /// <summary>
    /// Represents a collection of services.
    /// </summary>
    public interface IServiceCollection
    {
        /// <summary>
        /// Adds a new singleton service that may be resolved as <typeparamref name="TService"/>. <br/>
        /// Singletons have just one instance across the service container.
        /// </summary>
        /// <typeparam name="TService">The type that the service should be resolved as.</typeparam>
        /// <typeparam name="TImplementation">The type that implements the service.</typeparam>
        /// <returns>Returns this service collection.</returns>
        IServiceCollection AddSingleton<TService, TImplementation>() 
            where TImplementation : class, TService;

        /// <summary>
        /// Adds a new transient service that may be resolved as <typeparamref name="TService"/>. <br/>
        /// Transient services are re-created every time they are used so each depending class will get a new instance.
        /// </summary>
        /// <typeparam name="TService">The type that the service should be resolved as.</typeparam>
        /// <typeparam name="TImplementation">The type that implements the service.</typeparam>
        /// <returns>Returns this service collection.</returns>
        IServiceCollection AddTransient<TService, TImplementation>()
           where TImplementation : class, TService;

        /// <summary>
        /// Builds a service provider that contains all the services added to this collection.
        /// </summary>
        /// <returns>Returns a service provider that may be used to resolve these services.</returns>
        IServiceProvider BuildServiceProvider();
    }
}
