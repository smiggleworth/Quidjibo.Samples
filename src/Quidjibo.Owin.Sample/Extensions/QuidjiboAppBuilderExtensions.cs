using System;
using Microsoft.Owin.BuilderProperties;
using Owin;
using Quidjibo.Servers;

namespace Quidjibo.Owin.Sample.Extensions
{
    public static class QuidjiboAppBuilderExtensions
    {
        public static IAppBuilder UseQuidjiboServer(this IAppBuilder appBuilder, Func<Quidjibo> QuidjiboServer)
        {
            var props = new AppProperties(appBuilder.Properties);
            var server = QuidjiboServer();
            props.OnAppDisposing.Register(server.Dispose);
            server.Start();
            return appBuilder;
        }
    }
}