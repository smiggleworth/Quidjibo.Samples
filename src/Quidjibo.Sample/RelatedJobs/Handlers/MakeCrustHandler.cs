using System;
using System.Threading;
using System.Threading.Tasks;
using Quidjibo.Handlers;
using Quidjibo.Models;
using Quidjibo.Sample.RelatedJobs.Commands;

namespace Quidjibo.Sample.RelatedJobs.Handlers
{
    public class MakeCrustHandler : IWorkHandler<MakeCrustCommand> {
        public Task ProcessAsync(MakeCrustCommand command, IProgress<Tracker> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}