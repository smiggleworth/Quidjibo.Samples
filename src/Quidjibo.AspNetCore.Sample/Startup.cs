﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quidjibo.AspNet.Extensions;
using Quidjibo.Autofac.Modules;
using Quidjibo.Clients;
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
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddControllersAsServices();


            var assemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new QuidjiboModule(assemblies));
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
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


            var quidjiboBuilder = new QuidjiboBuilder()
                .UseSqlServer(new SqlServerQuidjiboConfiguration
                {
                    // load your connection string
                    ConnectionString = "Server=localhost;Database=SampleDb;Trusted_Connection=True;",

                    // the queues the worker should be polling
                    Queues = new List<string>
                    {
                        "default"
                    },

                    // the delay between batches
                    PollingInterval = 10,

                    // maximum concurrent requests
                    Throttle = 2,
                    SingleLoop = true
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