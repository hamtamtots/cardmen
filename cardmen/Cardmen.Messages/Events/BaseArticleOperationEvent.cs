using NServiceBus;
using System;

namespace Cardmen.Messages.Events
{
    public class BaseArticleOperationEvent : IEvent
    {

        public Guid ArticleId { get; set; }
    }
}
