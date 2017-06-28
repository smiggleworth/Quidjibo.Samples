using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quidjibo.Clients;
using Quidjibo.Sample.Jobs;

namespace Quidjibo.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuidjiboClient _publisherClient;

        public HomeController(IQuidjiboClient publisherClient)
        {
            _publisherClient = publisherClient;
        }


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> PublishJobs()
        {


            var cmd = new ExampleJob.Command("Hello World!");
            await _publisherClient.PublishAsync(cmd);


            return View("Index");
        }
    }
}