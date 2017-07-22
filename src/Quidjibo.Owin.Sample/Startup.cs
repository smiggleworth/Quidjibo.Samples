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
using Quidjibo.SqlServer.Extensions;
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


            var quidjiboBuilder = new QuidjiboBuilder()
                .UseSqlServer(new SqlServerQuidjiboConfiguration
                {
                    // load your connection string
                    ConnectionString = "Server=localhost;Database=SampleDb;Trusted_Connection=True;",

                    // the queues the worker should be polling
                    Queues = new List<string>
                    {
                        "default",
                        "other"
                    },

                    // the delay between batches
                    PollingInterval = 10,

                    // maximum concurrent requests
                    Throttle = 2,
                    SingleLoop = true
                });

            app.UseQuidjibo(quidjiboBuilder);
        }
    }
}