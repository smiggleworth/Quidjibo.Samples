using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quidjibo.AspNet.Extensions;
using Quidjibo.Factories;
using Quidjibo.Sample.Business;
using Quidjibo.SqlServer.Configurations;
using Quidjibo.SqlServer.Factories;

namespace Quidjibo.AspNetCore.Sample
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            // UserWorkServer will automatically start and stop the work server
            app.UseWorkServer(() =>
            {
                var connectionString = Configuration.GetConnectionString("SampleDb");
                var sqlWorkProviderFactory = new SqlWorkProviderFactory(connectionString);
                var sqlScheduleProviderFactory = new SqlScheduleProviderFactory(connectionString);
                var sqlProgressProviderFactory = new SqlProgressProviderFactory(connectionString);
                var configuration = new SqlServerWorkConfiguration
                {
                    Queues = new List<string>
                    {
                        "default",
                        "other-stuff"
                    },
                    PollingInterval = 10,
                    Throttle = 2
                };
                return WorkServerFactory.Create(typeof(BusinessLogic).GetTypeInfo().Assembly, configuration,
                    sqlWorkProviderFactory,
                    sqlScheduleProviderFactory,
                    sqlProgressProviderFactory,
                    loggerFactory);
            });
        }
    }
}