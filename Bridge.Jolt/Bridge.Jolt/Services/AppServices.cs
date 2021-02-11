using Jolt.Abstractions;
using Jolt.Navigation;
using Jolt.StandardServices.Default;
using System;

namespace Jolt
{
    /// <summary>
    /// Implements a default service provider.
    /// </summary>
    public static class AppServices
    {
        #region Constructors

        /// <summary>
        /// Static initialization for the <see cref="AppServices"/> class to ensure that the default Jolt- services are
        /// available if services are not configured.
        /// </summary>
        static AppServices()
        {
            AddDefaultJoltServices(defaultJoltServices);
            Default = defaultJoltServices.BuildServiceProvider();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current global service provider for this application.
        /// </summary>
        public static IServices Default { get; private set; } 

        #endregion

        #region Methods

        /// <summary>
        /// Sets a service provider to use for this application.
        /// </summary>
        /// <param name="provider"></param>
        public static void SetServiceProvider(IServices provider)
        {
            Default = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Uses a startup class to configure the services for this application. This is the recommended approach.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void UseStartup<T>()
            where T : class, IStartup
        {
            ServiceCollection services = defaultJoltServices.CopyWithoutInstances();;

            IServices startupProvider = services.BuildServiceProvider();
            IStartup startup = ActivatorUtilities.CreateInstance<T>(startupProvider);
            startup.ConfigureServices(services);

            IServices provider = services.BuildServiceProvider();
            SetServiceProvider(provider);

            startup.Configure(provider);
        }

        #endregion

        #region Privates

        /// <summary>
        /// Adds all the Jolt- default services to the application.
        /// </summary>
        /// <param name="services"></param>
        static void AddDefaultJoltServices(IServiceCollection services)
        {
            services
                .AddSingleton<IApiClient, JsonApiClient>()
                .AddSingleton<IBrowserHistory, NativeBrowserHistory>()
                .AddSingleton<IErrorHandler, DefaultErrorHandler>()
                .AddSingleton<IGlobalEvents, DefaultGlobalEvents>()
                .AddSingleton<IJoltImageProvider, DefaultJoltImageProvider>()
                .AddSingleton<IJoltLocale, JoltEnglishLocale>();
        }


        static readonly ServiceCollection defaultJoltServices = new ServiceCollection();

        #endregion
    }
}
