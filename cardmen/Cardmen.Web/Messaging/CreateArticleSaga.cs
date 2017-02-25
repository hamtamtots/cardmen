using System;
using System.Threading.Tasks;
using Cardmen.Messages.Commands;
using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Cardmen.Web.Messaging
{
    public class CreateArticleSaga : 
        BaseArticleOperationSaga,
        IAmStartedByMessages<CreateArticle>
    {

        public CreateArticleSaga(ILoggerFactory loggerFactory) : 
            base(loggerFactory.CreateLogger<CreateArticleSaga>())
        {
        }


        public Task Handle(CreateArticle message, IMessageHandlerContext context)
        {
            context.Publish(new ArticleCreated() { ArticleId = message.ArticleId });
            return Task.CompletedTask;
        }

    }
}
