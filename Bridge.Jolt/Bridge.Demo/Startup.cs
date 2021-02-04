using Bridge.Demo.CustomService;
using Bridge.Jolt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services
                .AddSingleton<IMyCustomService, MyCustomService>()
                .AddSingleton<IOtherService, OtherService>();
        }

        public void Configure()
        {
        }
    }
}
