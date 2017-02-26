﻿using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

namespace Cardmen.Storage
{
    class ArticleCreatedHandler : IHandleMessages<ArticleCreated>
    {

        ILogger _log;


        public ArticleCreatedHandler(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<ArticleCreatedHandler>();
        }


        public Task Handle(ArticleCreated message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Article created event received for article {message.ArticleId}");
            // do actual storage logic here, have async
            context.Publish(new ArticleStoreUpdated() { ArticleId = message.ArticleId });
            return Task.CompletedTask;
        }
    }
}
