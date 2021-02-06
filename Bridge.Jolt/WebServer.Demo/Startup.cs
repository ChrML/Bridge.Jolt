using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Bridge.Jolt.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // If environment variable for a different "/bridge" route is provided, add a PhysicalFileProvider for it.
            // Reason we need this is to provide the Bridge.NET front-end files while debugging from Visual Studio.
            string alternateBridgeNetPath = Environment.GetEnvironmentVariable("BRIDGE_PATH");
            if (!String.IsNullOrEmpty(alternateBridgeNetPath))
            {
                StaticFileOptions staticFileOpt1 = new StaticFileOptions { RequestPath = "/bridge" };
                staticFileOpt1.FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), alternateBridgeNetPath));
                app.UseStaticFiles(staticFileOpt1);

                StaticFileOptions staticFileOpt2 = new StaticFileOptions { RequestPath = "/bridge" };
                staticFileOpt2.FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), alternateBridgeNetPath.Replace("/bridge", "/Jolt")));
                app.UseStaticFiles(staticFileOpt2);
            }
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
