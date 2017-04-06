using System;
using System.Threading;
using System.Threading.Tasks;
using Quidjibo.Commands;
using Quidjibo.Handlers;
using Quidjibo.Models;

namespace Quidjibo.Sample.RelatedJobs.Handlers
{
    public class PreheatHandler : IWorkHandler<IWorkCommand>
    {
        public Task ProcessAsync(IWorkCommand command, IProgress<Tracker> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}