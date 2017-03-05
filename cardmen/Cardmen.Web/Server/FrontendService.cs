using Cardmen.Messages.Commands;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Cardmen.Web.Server
{
    public class FrontendService : IDisposable, IArticleRepository
    {

        private ILogger _log;
        private IEndpointInstance _endpoint;


        public FrontendService(ILoggerFactory logFactory, IEndpointInstance endpoint)
        {
            _log = logFactory.CreateLogger<FrontendService>();
            _endpoint = endpoint;
            _log.LogInformation("Service created");
            Start();
        }


        public void Dispose()
        {
            Stop();
        }


        public async Task CreateArticleAsync(Guid articleId, string operationKey)
        {
            await _endpoint.SendLocal(new CreateArticle() { ArticleId = articleId, OperationKey = operationKey });
            _log.LogInformation($"Article creation command sent, id: {articleId}");
        }


        private void Start()
        {
            _log.LogInformation("Service starting");
            _log.LogInformation("Service started");
        }


        private void Stop()
        {
            _log.LogInformation("Service stopping");
            _endpoint.Stop().Wait();
            _log.LogInformation("Service stopped");
        }
    }
}
