using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cardmen.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Cardmen.Storage
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

            using (var service = serviceProvider.GetService<StorageService>())
            {
                WaitForConsoleExitAsync().Wait();
            }
        }


        static EndpointConfiguration ConfigureEndpoint(IConfigurationRoot configuration)
        {
            var endpointConfig = new EndpointConfiguration(configuration["ENDPOINT_NAME"] ?? "Storage.Endpoint");
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
                .AddSingleton<StorageService>()
                .AddSingleton<IMongoClient>(_ => new MongoClient(config["MONGO_CONNECTION_STRING"] ?? "mongodb://192.168.99.100:27017"))
                .AddTransient(serviceProvider =>
                {
                    return serviceProvider
                        .GetService<IMongoClient>()
                        .GetDatabase(config["MONGO_DB_NAME"] ?? "cardmen")
                        .GetCollection<Article>(config["MONGO_ARTICLE_COLLECTION_NAME"] ?? "articles");
                })
                .AddTransient<IArticleStorageRepository, MongoArticleRepository>();

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