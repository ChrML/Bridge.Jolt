using System;
using System.Collections.Generic;

namespace Bridge.Jolt.Services
{
    /// <summary>
    /// Implements a service collection that may be used to add services to a service container.
    /// </summary>
    public class ServiceCollection: IServiceCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCollection"/> class.
        /// </summary>
        public ServiceCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCollection"/> class.
        /// </summary>
        /// <param name="input"></param>
        ServiceCollection(Dictionary<Type, ServiceDescriptor> input)
        {
            this.services = input ?? throw new ArgumentNullException(nameof(input));
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public IServiceCollection AddSingleton<TService, TImplementation>()
            where TImplementation : class, TService
        {
            ServiceDescriptor descriptor = new ServiceDescriptor
            (
                implementingType: typeof(TImplementation),
                serviceType: typeof(TService),
                singleton: true
            );

            if (this.services.TryAdd(typeof(TService), descriptor))
            {
                return this;
            }
            else
            {
                throw new InvalidOperationException($"Already a service registered with type {typeof(TService).FullName}.");
            }
        }

        /// <inheritdoc/>
        public IServiceCollection AddTransient<TService, TImplementation>() 
            where TImplementation : class, TService
        {
            ServiceDescriptor descriptor = new ServiceDescriptor
            (
                implementingType: typeof(TImplementation),
                serviceType: typeof(TService),
                singleton: false
            );

            if (this.services.TryAdd(typeof(TService), descriptor))
            {
                return this;
            }
            else
            {
                throw new InvalidOperationException($"Already a service registered with type {typeof(TService).FullName}.");
            }
        }

        /// <inheritdoc/>
        public IServiceProvider BuildServiceProvider()
        {
            Dictionary<Type, ServiceDescriptor> copy = new Dictionary<Type, ServiceDescriptor>();
            foreach (KeyValuePair<Type, ServiceDescriptor> service in this.services)
            {
                copy[service.Key] = service.Value.CopyWithoutInstance();
            }

            return new ServiceProvider(copy);
        }

        #endregion

        #region Privates

        /// <summary>
        /// Makes a copy of this service collection.
        /// </summary>
        internal ServiceCollection CopyWithoutInstances()
        {
            Dictionary<Type, ServiceDescriptor> copy = new Dictionary<Type, ServiceDescriptor>();
            foreach (KeyValuePair<Type, ServiceDescriptor> service in this.services)
            {
                copy[service.Key] = service.Value.CopyWithoutInstance();
            }

            return new ServiceCollection(copy);
        }


        readonly Dictionary<Type, ServiceDescriptor> services = new Dictionary<Type, ServiceDescriptor>();

        #endregion
    }
}
