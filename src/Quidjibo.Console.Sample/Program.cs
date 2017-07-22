using System;
using System.Collections.Generic;
using System.Threading;
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
                        "default",
                        "other"
                    },

                    // the delay between batches
                    PollingInterval = 10,

                    // maximum concurrent requests
                    Throttle = 2,
                    SingleLoop = true
                });
            var cts = new CancellationTokenSource();
            var client = quidjiboBuilder.BuildClient();
            var workServer = quidjiboBuilder.BuildServer();
            workServer.Start();
            System.Console.CancelKeyPress += (s, e) =>
            {
                workServer.Dispose();
                cts.Cancel();
            };
            PublishAway(cts.Token).Wait(cts.Token);
        }

        private static async Task PublishAway(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // use a the static client
                var client = (IQuidjiboClient)QuidjiboClient.Instance;
                var logic = new BusinessLogic(client);
                await logic.BusinessWithFireAndForget();
                await Task.Delay(TimeSpan.FromSeconds(1), token);
            }
        }

        private static async Task ScheduleSomthing(IQuidjiboClient client)
        {
            await client.ScheduleAsync("GitEmojis", new GitEmojisCommand(), new Cron("0 22 * * 6"));
        }
    }
}