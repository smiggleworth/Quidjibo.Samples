
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Quidjibo.Clients;
using Quidjibo.Models;
using Quidjibo.Sample.Business;
using Quidjibo.Sample.SplitThemUp.Commands;
using Quidjibo.SqlServer.Configurations;
using Quidjibo.SqlServer.Extensions;

namespace Quidjibo.Console.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
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


            var client = quidjiboBuilder.BuildClient();


            var task = PublishAway(client);




            using (var workServer = quidjiboBuilder.BuildServer())
            {
                workServer.Start();
                System.Console.WriteLine("Press any key to exit.");
                System.Console.Read();
            }
        }


        private static async Task PublishAway(IQuidjiboClient client)
        {

            var logic = new BusinessLogic(client);
            while (true)
            {
                await logic.BusinessWithFireAndForget();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private static async Task ScheduleSomthing(IQuidjiboClient client)
        {
            await client.ScheduleAsync("GitEmojis", new GitEmojisCommand(), new Cron("0 22 * * 6"));
        }
    }
}