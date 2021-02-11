using System;
using System.Reflection;

namespace Jolt
{
    /// <summary>
    /// Provides extension methods for <see cref="IServices"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Resolves a required service of the given type.
        /// </summary>
        /// <typeparam name="TService">The service to resolve.</typeparam>
        /// <returns>Returns the instance of the service to use.</returns>
        public static TService Resolve<TService>(this IServices provider) 
            where TService : class
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            return (TService)provider.GetService(typeof(TService)) ?? throw new InvalidOperationException("No service registered for " + typeof(TService).FullName + ".");
        }
    }
}
