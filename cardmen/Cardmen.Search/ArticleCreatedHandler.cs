using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

namespace Cardmen.Search
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
            // do actual index logic here, have async
            context.Publish(new ArticleIndexUpdated() { ArticleId = message.ArticleId, OperationKey = message.OperationKey });
            return Task.CompletedTask;
        }
    }
}
