using NServiceBus.ObjectBuilder.Common;
using System;
using NServiceBus;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cardmen.Client
{
    static class Program
    {
        static void Main(string[] args)
        {
            var endpointConfig = ConfigureEndpoint();
            var services = new ServiceCollection()
                .AddLogging()
                .AddNServiceBus(() => endpointConfig)
                .AddSingleton<Service>();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            var container = builder.Build();

            endpointConfig.UseContainer<AutofacBuilder>(customizations => customizations.ExistingLifetimeScope(container));
            var serviceProvider = new AutofacServiceProvider(container);

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddConsole();

            using (var service = serviceProvider.GetService<Service>())
            {
                WaitForConsoleExitAsync().Wait();
            }
        }


        static EndpointConfiguration ConfigureEndpoint()
        {
            var endpointConfig = new EndpointConfiguration("Test.Endpoint");
            var transport = endpointConfig.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=192.168.99.100"); // move to env var
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();
            return endpointConfig;
        }


        static IServiceCollection AddNServiceBus(this IServiceCollection services, Func<EndpointConfiguration> getEndpointConfig)
        {
            return services.AddSingleton(_ => Endpoint.Start(getEndpointConfig()).Result);
        }


        static async Task WaitForConsoleExitAsync()
        {
            TaskCompletionSource<object> consoleExit = new TaskCompletionSource<object>();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                consoleExit.SetResult(null);
            };
            await consoleExit.Task;
        }
    }
}