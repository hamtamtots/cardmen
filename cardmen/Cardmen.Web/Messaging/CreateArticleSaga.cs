using System;
using System.Threading.Tasks;
using Cardmen.Messages.Commands;
using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Cardmen.Web.Server;

namespace Cardmen.Web.Messaging
{
    public class CreateArticleSaga : 
        BaseArticleOperationSaga,
        IAmStartedByMessages<CreateArticle>
    {

        public CreateArticleSaga(ILoggerFactory loggerFactory, IArticleClientProxy articleClientProxy) : 
            base(loggerFactory.CreateLogger<CreateArticleSaga>(), articleClientProxy)
        {
        }


        public Task Handle(CreateArticle message, IMessageHandlerContext context)
        {
            Data.ArticleId = message.ArticleId;
            LogInfo("Saga started");
            context.Publish(new ArticleCreated() { ArticleId = message.ArticleId, OperationKey = message.OperationKey });
            return Task.CompletedTask;
        }


        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ArticleOperationData> mapper)
        {
            base.ConfigureHowToFindSaga(mapper);
            mapper
                .ConfigureMapping<CreateArticle>(msg => msg.OperationKey)
                .ToSaga(data => data.OperationKey);
        }

    }
}
