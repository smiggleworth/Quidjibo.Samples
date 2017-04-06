using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using Quidjibo.Factories;
using Quidjibo.Owin.Sample;
using Quidjibo.Owin.Sample.Extensions;
using Quidjibo.SqlServer.Configurations;
using Quidjibo.SqlServer.Factories;

[assembly: OwinStartup(typeof(Startup))]
namespace Quidjibo.Owin.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // UserWorkServer will automatically start and stop the work server
            app.UseWorkServer(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["SampleDb"].ConnectionString;
                var workProviderFactory = new SqlWorkProviderFactory(connectionString);
                var scheduleProviderFactory = new SqlScheduleProviderFactory(connectionString);
                var progressProviderFactory = new SqlProgressProviderFactory(connectionString);
                var configuration = new SqlServerWorkConfiguration
                {
                    Queues = new List<string>
                    {
                        "default",
                        "other-stuff"
                    },
                    PollingInterval = 30,
                    Throttle = 2
                };
                return WorkServerFactory.Create(typeof(Startup).Assembly, configuration, workProviderFactory,
                    scheduleProviderFactory, progressProviderFactory);
            });
        }
    }
}