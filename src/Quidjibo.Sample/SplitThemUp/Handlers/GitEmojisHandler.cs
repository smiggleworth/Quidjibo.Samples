using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Quidjibo.Handlers;
using Quidjibo.Models;
using Quidjibo.Sample.SplitThemUp.Commands;

namespace Quidjibo.Sample.SplitThemUp.Handlers
{
    public class GitEmojisHandler : IQuidjiboHandler<GitEmojisCommand>
    {
        public async Task ProcessAsync(GitEmojisCommand command, IProgress<Tracker> progress, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var file = new FileStream("emojis.json", FileMode.Create, FileAccess.ReadWrite))
            {
                var response = await client.GetAsync("https://api.github.com/emojis", cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    await response.Content.CopyToAsync(file);
                }
            }
        }
    }
}