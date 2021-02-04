using Bridge.Demo.CustomService;
using Bridge.Jolt;
using Bridge.Jolt.Services;
using System;

namespace Bridge.Demo
{
    [Reflectable(true)]
    class Startup: IStartup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add your services to the container here.

            services
                .AddSingleton<IMyCustomService, MyCustomService>()
                .AddSingleton<IOtherService, OtherService>();
        }

        public void Configure(Jolt.IServiceProvider provider)
        {
            // Perform additional configuration here.
        }
    }
}
