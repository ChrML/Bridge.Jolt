using Jolt.Demo.CustomService;
using Jolt.Services;
using Bridge;

namespace Jolt.Demo
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

        public void Configure(Jolt.IServices provider)
        {
            // Perform additional configuration here.
        }
    }
}
