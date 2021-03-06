﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cardmen.Web.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;

namespace Cardmen.Web.Server
{
    public class Startup
    {

        private IContainer _container;
        

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var endpointConfig = ConfigureEndpoint(config);

            services
                .AddLogging()
                .AddSingleton(_ => Endpoint.Start(endpointConfig).Result)
                .AddSingleton(config)
                .AddSingleton<IArticleRepository, FrontendService>()
                .AddSingleton<IArticleClientProxy, ArticleHubClientProxy>()
                .AddSignalR(options => options.Hubs.EnableDetailedErrors = true);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            _container = builder.Build();
            endpointConfig.UseContainer<AutofacBuilder>(customizations => customizations.ExistingLifetimeScope(_container));
            return new AutofacServiceProvider(_container);
        }


        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseSignalR()
                .UseWebSockets();

            appLifetime.ApplicationStopped.Register(() => _container.Dispose());
        }


        private EndpointConfiguration ConfigureEndpoint(IConfigurationRoot configuration)
        {
            var endpointConfig = new EndpointConfiguration(configuration["ENDPOINT_NAME"] ?? "Frontend.Endpoint");
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
    }
}
