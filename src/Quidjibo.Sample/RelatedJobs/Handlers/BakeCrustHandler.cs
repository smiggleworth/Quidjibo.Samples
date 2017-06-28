using System;
using System.Threading;
using System.Threading.Tasks;
using Quidjibo.Handlers;
using Quidjibo.Models;
using Quidjibo.Sample.RelatedJobs.Commands;

namespace Quidjibo.Sample.RelatedJobs.Handlers
{
    public class BakeCrustHandler : IQuidjiboHandler<BakeCommand> {
        public Task ProcessAsync(BakeCommand command, IProgress<Tracker> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}