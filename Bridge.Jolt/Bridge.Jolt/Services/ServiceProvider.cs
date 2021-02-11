using System;
using System.Collections.Generic;

namespace Jolt
{
    /// <summary>
    /// Implements the service provider of the default dependency-injection implementation.
    /// </summary>
    class ServiceProvider: IServices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProvider"/> class.
        /// </summary>
        /// <param name="services">Contains the services that we may use.</param>
        internal ServiceProvider(Dictionary<Type, ServiceDescriptor> services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (this.services.TryGetValue(serviceType, out ServiceDescriptor value))
            {
                return value.GetOrCreateInstance(this);
            }
            else
            {
                return null;
            }
        }


        readonly Dictionary<Type, ServiceDescriptor> services = new Dictionary<Type, ServiceDescriptor>();
    }
}
