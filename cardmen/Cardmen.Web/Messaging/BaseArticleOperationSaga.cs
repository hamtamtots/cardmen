﻿using Cardmen.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using System;
using Cardmen.Messages.Commands;

namespace Cardmen.Web.Messaging
{
    public abstract class BaseArticleOperationSaga :
        Saga<ArticleOperationData>,
        IHandleMessages<ArticleStoreUpdated>,
        IHandleMessages<ArticleIndexUpdated>
    {

        private ILogger _log;
        

        public BaseArticleOperationSaga(ILogger log)
        {
            _log = log;
            LogInfo($"Saga created");
        }


        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ArticleOperationData> mapper)
        {
            mapper.ConfigureMapping<ArticleStoreUpdated>(msg => msg.ArticleId).ToSaga(data => data.ArticleId);
            mapper.ConfigureMapping<ArticleIndexUpdated>(msg => msg.ArticleId).ToSaga(data => data.ArticleId);
        }



        public Task Handle(ArticleStoreUpdated message, IMessageHandlerContext context)
        {
            Data.IsArticleStored = true;
            LogInfo("Article stored");
            return Task.CompletedTask;
        }


        public Task Handle(ArticleIndexUpdated message, IMessageHandlerContext context)
        {
            Data.IsArticleIndexed = true;
            LogInfo("Article indexed");
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
                MarkAsComplete();
            }
        }
    }
}