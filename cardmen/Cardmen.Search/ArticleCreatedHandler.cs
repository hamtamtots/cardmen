using Cardmen.Messages;
using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

namespace Cardmen.Search
{
    class ArticleCreatedHandler : IHandleMessages<ArticleCreated>
    {

        private ILogger _log;
        private IArticleIndexer _articleIndexer;


        public ArticleCreatedHandler(ILoggerFactory loggerFactory, IArticleIndexer articleIndexer)
        {
            _log = loggerFactory.CreateLogger<ArticleCreatedHandler>();
            _articleIndexer = articleIndexer;
        }


        public async Task Handle(ArticleCreated message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Article created event received for article {message.ArticleId}");
            await _articleIndexer.IndexArticle(new Article() { Id = message.ArticleId });
            await context.Publish(new ArticleIndexUpdated() { ArticleId = message.ArticleId, OperationKey = message.OperationKey });
        }
    }
}
