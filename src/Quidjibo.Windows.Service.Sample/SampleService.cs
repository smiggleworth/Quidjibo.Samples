using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;


namespace Quidjibo.Windows.Service.Sample
{
    public partial class SampleService : ServiceBase
    {
        private IQuidjiboServer _workServer;

        public SampleService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var loggerFactory = new LoggerFactory().AddConsole().AddDebug();

            loggerFactory.AddProvider(new ConsoleLoggerProvider((text, logLevel) => logLevel >= LogLevel.Debug, true));


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

            _workServer.Start();
        }

        protected override void OnStop()
        {
            _workServer?.Dispose();
        }
    }
}