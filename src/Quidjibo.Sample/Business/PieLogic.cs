using System.Threading.Tasks;
using Quidjibo.Clients;
using Quidjibo.Commands;
using Quidjibo.Factories;
using Quidjibo.Sample.RelatedJobs.Commands;

namespace Quidjibo.Sample.Business
{
  public  class PieLogic
    {



        private  async Task MakePies()
        {
            var client = QuidjiboClient.Instance;
            var workflow = new WorkflowCommand(new PreheatCommand())
                .Then(i => new IQuidjiboCommand[]
                {
                    new MakeCrustCommand(),
                    new MakeFillingCommand()
                })
                .Then(i => new BakeCommand())
                .Then(i => new PackageCommand());

            await client.PublishAsync(workflow);

        }


    }
}
