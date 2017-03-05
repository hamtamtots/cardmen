using NServiceBus;
using System;

namespace Cardmen.Web.Messaging
{
    public class ArticleOperationData : ContainSagaData
    {

        public Guid ArticleId { get; set; }


        public string OperationKey { get; set; }


        public bool IsArticleStored { get; set; } = false;


        public bool IsArticleIndexed { get; set; } = false;
    }
}
