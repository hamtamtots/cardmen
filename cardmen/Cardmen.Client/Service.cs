using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardmen.Client
{
    class Service : IDisposable
    {

        private ILogger _log;
        private IEndpointInstance _endpoint;


        public Service(ILoggerFactory logFactory, IEndpointInstance endpoint)
        {
            _log = logFactory.CreateLogger<Service>();
            _endpoint = endpoint;
            _log.LogInformation("Service created");
            Start();
        }
        

        public void Dispose()
        {
            Stop();
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
