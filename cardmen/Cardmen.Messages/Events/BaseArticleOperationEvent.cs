using System;

namespace Cardmen.Messages.Events
{
    public class BaseArticleOperationEvent
    {

        public Guid ArticleId { get; set; }


        public string OperationKey { get; set; }
    }
}
