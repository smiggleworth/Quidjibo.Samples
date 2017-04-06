using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Quidjibo.Factories;
using Quidjibo.Servers;
using Quidjibo.SqlServer.Configurations;
using Quidjibo.SqlServer.Factories;

namespace Quidjibo.Windows.Service.Sample
{
    public partial class SampleService : ServiceBase
    {
        private IWorkServer _workServer;

        public SampleService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var loggerFactory = new LoggerFactory().AddConsole().AddDebug();

            loggerFactory.AddProvider(new ConsoleLoggerProvider((text, logLevel) => logLevel >= LogLevel.Debug, true));


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
            _workServer = WorkServerFactory.Create(
                typeof(Program).Assembly,
                configuration,
                workProviderFactory,
                scheduleProviderFactory,
                progressProviderFactory,
                loggerFactory);

            _workServer.Start();
        }

        protected override void OnStop()
        {
            _workServer?.Dispose();
        }
    }
}