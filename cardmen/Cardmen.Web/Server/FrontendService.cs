using Cardmen.Messages.Commands;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;

namespace Cardmen.Web.Server
{
    public class FrontendService : IDisposable
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


        public void CreateArticle(Guid articleId)
        {
            _endpoint.SendLocal(new CreateArticle() { ArticleId = articleId });
        }


        private void Start()
        {
            _log.LogInformation("Service starting");
            // endpoint should have been created from DI
            // do shit now? send stuff or just do nothing, let handlers worry about shit
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
