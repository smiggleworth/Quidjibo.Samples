using System;
using Microsoft.Owin.BuilderProperties;
using Owin;
using Quidjibo.Servers;

namespace Quidjibo.Owin.Sample.Extensions
{
    public static class QuidjiboAppBuilderExtensions
    {
        public static IAppBuilder UseQuidjibo(this IAppBuilder appBuilder, QuidjiboBuilder quidjiboBuilder)
        {
            var props = new AppProperties(appBuilder.Properties);
            var server = quidjiboBuilder.BuildServer();
            quidjiboBuilder.BuildClient();
            props.OnAppDisposing.Register(server.Dispose);
            server.Start();
            return appBuilder;
        }
    }
}