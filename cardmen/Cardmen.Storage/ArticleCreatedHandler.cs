using Cardmen.Messages;
using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

namespace Cardmen.Storage
{
    class ArticleCreatedHandler : IHandleMessages<ArticleCreated>
    {

        ILogger _log;
        IArticleStorageRepository _articleRepository;


        public ArticleCreatedHandler(ILoggerFactory loggerFactory, IArticleStorageRepository articleRepository)
        {
            _log = loggerFactory.CreateLogger<ArticleCreatedHandler>();
            _articleRepository = articleRepository;
        }


        public async Task Handle(ArticleCreated message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Article created event received for article {message.ArticleId}");
            await _articleRepository.InsertArticle(new Article() { Id = message.ArticleId });
            await context.Publish(new ArticleStoreUpdated() { ArticleId = message.ArticleId, OperationKey = message.OperationKey });
        }
    }
}
