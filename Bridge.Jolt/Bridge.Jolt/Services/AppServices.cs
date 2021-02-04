using Bridge.Jolt.Abstractions;
using Bridge.Jolt.Services.Default;
using System;
using System.Collections.Generic;

namespace Bridge.Jolt.Services
{
    /// <summary>
    /// Implements a default service provider.
    /// </summary>
    public sealed class AppServices: IServiceProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppServices"/> class.
        /// </summary>
        public AppServices()
        {
            // TODO: For now we just provide some very simple default services here.

            this
                .AddService<IErrorHandler, DefaultErrorHandler>()
                .AddService<IJoltImageProvider, DefaultJoltImageProvider>();
        }

        /// <summary>
        /// Gets the default service provider for this application.
        /// </summary>
        public static IServiceProvider Default { get; } = new AppServices();

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (this.services.TryGetValue(serviceType, out object value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }



        AppServices AddService<TService, TImplementation>()
            where TImplementation : TService, new()
        {
            TService implementation = new TImplementation();
            this.services.Add(typeof(TService), implementation);
            return this;
        }


        readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
    }
}
