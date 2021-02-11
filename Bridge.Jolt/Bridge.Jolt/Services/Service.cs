namespace Jolt
{
    /// <summary>
    /// Internal class for resolving services for use by Jolt- classes.
    /// </summary>
    static class Service
    {
        /// <summary>
        /// Attempts to resolve the given service.
        /// </summary>
        /// <typeparam name="T">The service type to resolve.</typeparam>
        /// <returns>Returns an instance of the service requested.</returns>
        public static T Resolve<T>() where T : class
        {
            return AppServices.Default.Resolve<T>();
        }
    }
}
