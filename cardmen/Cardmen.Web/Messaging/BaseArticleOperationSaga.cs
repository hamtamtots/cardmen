using Cardmen.Messages.Events;
using Cardmen.Web.Server;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cardmen.Web.Messaging
{
    public abstract class BaseArticleOperationSaga :
        Saga<ArticleOperationData>,
        IHandleMessages<ArticleStoreUpdated>,
        IHandleMessages<ArticleIndexUpdated>
    {

        private ILogger _log;
        private IArticleClientProxy _articleClientProxy;
        

        public BaseArticleOperationSaga(ILogger log, IArticleClientProxy articleClientProxy)
        {
            _log = log;
            _articleClientProxy = articleClientProxy;
        }


        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ArticleOperationData> mapper)
        {
            mapper
                .ConfigureMapping<ArticleStoreUpdated>(msg => msg.OperationKey)
                .ToSaga(data => data.OperationKey);
            mapper
                .ConfigureMapping<ArticleIndexUpdated>(msg => msg.OperationKey)
                .ToSaga(data => data.OperationKey);
        }



        public Task Handle(ArticleStoreUpdated message, IMessageHandlerContext context)
        {
            Data.IsArticleStored = true;
            LogInfo("Article stored");
            CheckForSagaCompletion();
            return Task.CompletedTask;
        }


        public Task Handle(ArticleIndexUpdated message, IMessageHandlerContext context)
        {
            Data.IsArticleIndexed = true;
            LogInfo("Article indexed");
            CheckForSagaCompletion();
            return Task.CompletedTask;
        }


        protected void LogInfo(string msg)
        {
            _log.LogInformation($"{Data.ArticleId} - {msg}");
        }


        private void CheckForSagaCompletion()
        {
            if(Data.IsArticleStored && Data.IsArticleIndexed)
            {
                LogInfo("Saga complete");
                _articleClientProxy.NotifyArticleOperationSuccessful(Data.ArticleId, Data.OperationKey);
                MarkAsComplete();
            }
        }
    }
}
