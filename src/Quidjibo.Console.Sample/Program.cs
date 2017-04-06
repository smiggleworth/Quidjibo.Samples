using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Quidjibo.Clients;
using Quidjibo.Commands;
using Quidjibo.Factories;
using Quidjibo.Models;
using Quidjibo.Sample.Business;
using Quidjibo.Sample.Jobs;
using Quidjibo.Sample.SplitThemUp.Commands;
using Quidjibo.SqlServer.Configurations;
using Quidjibo.SqlServer.Factories;

namespace Quidjibo.Console.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
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
                PollingInterval = 10,
                Throttle = 2
            };

            var task = PublishAway(workProviderFactory);




            using (var workServer = WorkServerFactory.Create(
                typeof(BusinessLogic).Assembly,
                configuration,
                workProviderFactory,
                scheduleProviderFactory,
                progressProviderFactory,
                loggerFactory))
            {
                workServer.Start();
                System.Console.WriteLine("Press any key to exit.");
                System.Console.Read();
            }
        }


        private static async Task PublishAway(IWorkProviderFactory factory)
        {
            var publisher = new PublisherClient(factory);
            var logic = new BusinessLogic(publisher);
            while (true)
            {
                await logic.BusinessWithFireAndForget();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private static async Task ScheduleSomthing(IScheduleProviderFactory factory)
        {
            var s = new SchedulerClient(factory);
            await s.ScheduleAsync("GitEmojis", new GitEmojisCommand(), new Cron("0 22 * * 6"));
        }
    }
}