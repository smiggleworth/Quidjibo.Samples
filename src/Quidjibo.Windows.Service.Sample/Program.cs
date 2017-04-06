using System.ServiceProcess;

namespace Quidjibo.Windows.Service.Sample
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new SampleService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}