using Jolt.Demo.CustomService;
using Jolt.Navigation;
using Bridge;
using Retyped;

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
                .AddSingleton<IOtherService, OtherService>()
                .SetSimpleNavigatorIn(Html.GetById<dom.HTMLDivElement>("Demo-Root"));
        }

        public void Configure(Jolt.IServices provider)
        {
            // Perform additional configuration here.
        }
    }
}
