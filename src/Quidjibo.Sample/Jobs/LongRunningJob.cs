using System;
using System.Threading;
using System.Threading.Tasks;
using Quidjibo.Attributes;
using Quidjibo.Commands;
using Quidjibo.Handlers;
using Quidjibo.Models;

namespace Quidjibo.Sample.Jobs
{
    public class LongRunningJob
    {
        [QueueName("slow-jobs")]
        public class Command : IWorkCommand
        {
            public string Hello { get; }

            public Command(string hello)
            {
                Hello = hello;
            }
        }

        public class Handler : IWorkHandler<Command>
        {
            public async Task ProcessAsync(Command command, IProgress<Tracker> progress,
                CancellationToken cancellationToken)
            {
                await Task.Yield();

                var loopCounter = 0;

                while (!cancellationToken.IsCancellationRequested && loopCounter < 10)
                {
                    progress.Report(new Tracker(loopCounter / 10, "I am still working"));
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    loopCounter++;
                }
            }
        }
    }
}