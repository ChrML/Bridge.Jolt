namespace Jolt
{
    /// <summary>
    /// Provides a simplified access to the default service provider.
    /// </summary>
    public static class Services
    {
        /// <summary>
        /// Gets the current default service provider.
        /// </summary>
        public static IServices Default => AppServices.Default;

        /// <summary>
        /// Creates a new instance of the given class using the default service provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T New<T>() where T : class
        {
            return ActivatorUtilities.CreateInstance<T>(AppServices.Default);
        }

        /// <summary>
        /// Resolves the given service from the default service provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            return AppServices.Default.Resolve<T>();
        }
    }
}
