using NServiceBus.ObjectBuilder.Common;
using System;
using NServiceBus;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Cardmen.Client
{
    static class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var endpointConfig = ConfigureEndpoint(config);
            var serviceProvider = ConfigureDI(config, endpointConfig);

            serviceProvider.GetService<ILoggerFactory>().AddConsole();

            using (var service = serviceProvider.GetService<Service>())
            {
                WaitForConsoleExitAsync().Wait();
            }
        }


        static EndpointConfiguration ConfigureEndpoint(IConfigurationRoot configuration)
        {
            var endpointConfig = new EndpointConfiguration(configuration["ENDPOINT_NAME"] ?? "Test.Endpoint");
            var transport = endpointConfig.UseTransport<RabbitMQTransport>();
            transport.ConnectionString(configuration["RABBITMQ_CONNECTION_STRING"] ?? "host=192.168.99.100");
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();
            return endpointConfig;
        }


        static IServiceProvider ConfigureDI(IConfigurationRoot config, EndpointConfiguration endpointConfig)
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(_ => Endpoint.Start(endpointConfig).Result)
                .AddSingleton(config)
                .AddSingleton<Service>();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            var container = builder.Build();

            endpointConfig.UseContainer<AutofacBuilder>(customizations => customizations.ExistingLifetimeScope(container));
            return new AutofacServiceProvider(container);
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