namespace Jolt
{
    /// <summary>
    /// Provides an interface that may be inherited by Startup- configuration classes.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// The startup implementation should add its services to the service container within this call.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// The startup implementation may use the services here to perform additional initialization here.
        /// </summary>
        /// <param name="provider">The service provider built from the services added by <see cref="ConfigureServices(IServiceCollection)"/>.</param>
        void Configure(IServices provider);
    }
}
