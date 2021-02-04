using System;

namespace Bridge.Jolt
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Resolves a required service of the given type.
        /// </summary>
        /// <typeparam name="T">The service to resolve.</typeparam>
        /// <returns>Returns the instance of the service to use.</returns>
        public static T Resolve<T>(this IServiceProvider provider) where T : class
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            return (T)provider.GetService(typeof(T)) ?? throw new InvalidOperationException("No service registered for " + typeof(T).FullName + ".");
        }
    }
}
