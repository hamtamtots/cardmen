﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Cardmen.Search
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

            using (var service = serviceProvider.GetService<SearchService>())
            {
                WaitForConsoleExitAsync().Wait();
            }
        }


        static EndpointConfiguration ConfigureEndpoint(IConfigurationRoot configuration)
        {
            var endpointConfig = new EndpointConfiguration(configuration["ENDPOINT_NAME"] ?? "Search.Endpoint");
            var transport = endpointConfig.UseTransport<RabbitMQTransport>();
            transport.ConnectionString(configuration["RABBITMQ_CONNECTION_STRING"] ?? "host=192.168.99.100");
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.EnableInstallers();
            endpointConfig.MakeInstanceUniquelyAddressable(configuration["ENDPOINT_INSTANCE_ID"] ?? "_1");
            endpointConfig.Conventions()
                .DefiningCommandsAs(type => type.Namespace.Equals(typeof(Messages.Commands.CreateArticle).Namespace))
                .DefiningEventsAs(type => type.Namespace.Equals(typeof(Messages.Events.ArticleCreated).Namespace));
            return endpointConfig;
        }


        static IServiceProvider ConfigureDI(IConfigurationRoot config, EndpointConfiguration endpointConfig)
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(_ => Endpoint.Start(endpointConfig).Result)
                .AddSingleton(config)
                .AddSingleton<SearchService>()
                .AddSingleton<IElasticClient>(_ =>
                {
                    var elasticUri = new Uri(config["ELASTICSEARCH_URI"] ?? "http://192.168.99.100:9200");
                    var connectionSettings = new ConnectionSettings(elasticUri)
                        .DefaultIndex(config["ELASTICSEARCH_ARTICLE_INDEX_NAME"] ?? "articles");
                    return new ElasticClient(connectionSettings);
                })
                .AddTransient<IArticleIndexer, ElasticSearchArticleIndexer>();

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