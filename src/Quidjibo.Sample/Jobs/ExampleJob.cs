using System;
using System.Threading;
using System.Threading.Tasks;
using Quidjibo.Attributes;
using Quidjibo.Commands;
using Quidjibo.Handlers;
using Quidjibo.Models;

namespace Quidjibo.Sample.Jobs
{
    // if you want to keep the command and handler together


    public class ExampleJob
    {
        public class Command : IQuidjiboCommand
        {
            public string Blah { get; }

            public Command(string blah)
            {
                Blah = blah;
            }
        }

        public class Handler : IQuidjiboHandler<Command>
        {
            public async Task ProcessAsync(Command command, IProgress<Tracker> progress,
                CancellationToken cancellationToken)
            {
                await Task.Yield();

                progress.Report(new Tracker(0, "Begin"));

                await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);

                progress.Report(new Tracker(50, "Half way"));


                // thing that builds lots of strings
                var bigData = "";
                for (var i = 1; i < 1000; i++)
                {
                    bigData += "I am string {i}";
                }

                Console.WriteLine($"Processing {bigData}");

                progress.Report(new Tracker(100, "All done"));
            }
        }
    }
}