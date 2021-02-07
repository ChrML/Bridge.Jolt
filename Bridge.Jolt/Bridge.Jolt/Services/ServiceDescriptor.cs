using System;

namespace Jolt.Services
{
    /// <summary>
    /// Describes a single registered service, and keeps track of singleton instances.
    /// </summary>
    class ServiceDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptor"/> class.
        /// </summary>
        /// <param name="implementingType">The type that implements this service.</param>
        /// <param name="serviceType">The type that exposes this service.</param>
        /// <param name="singleton">If this is a singleton service.</param>
        /// <param name="singletonInstance">If a singleton instance is already configured it's provided here.</param>
        public ServiceDescriptor(Type implementingType, Type serviceType, bool singleton, object singletonInstance = null)
        {
            this.ImplementingType = implementingType ?? throw new ArgumentNullException(nameof(implementingType));
            this.ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            this.Singleton = singleton;
            this.singletonInstance = singletonInstance;
        }

        /// <summary>
        /// Creates a copy of this object without passing along instances that have been created after constructing.
        /// </summary>
        /// <returns></returns>
        public ServiceDescriptor CopyWithoutInstance()
        {
            // TODO: We should probably pass the one from the constructor, but not those created later.

            return new ServiceDescriptor
            (
                implementingType: this.ImplementingType,
                serviceType: this.ServiceType,
                singleton: this.Singleton
            );
        }

        /// <summary>
        /// Gets the type that implements this service.
        /// </summary>
        public Type ImplementingType { get; }

        /// <summary>
        /// Gets the type that exposes this service.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets if this is a singleton service.
        /// </summary>
        public bool Singleton { get; }

        /// <summary>
        /// Gets the service if it's a singleton and already exists, otherwise we create a new.
        /// </summary>
        /// <param name="provider">Service provider to use if the service has to be created.</param>
        /// <returns></returns>
        public object GetOrCreateInstance(IServices provider)
        {
            if (this.Singleton)
            {
                if (this.singletonInstance == null)
                {
                    this.singletonInstance = ActivatorUtilities.CreateInstance(provider, this.ImplementingType);
                }

                return this.singletonInstance;
            }
            else
            {
                return ActivatorUtilities.CreateInstance(provider, this.ImplementingType);
            }
        }

        object singletonInstance;
    }
}
