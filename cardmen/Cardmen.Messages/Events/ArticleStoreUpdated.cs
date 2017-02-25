using NServiceBus;
using System;

namespace Cardmen.Messages.Events
{
    public class ArticleStoreUpdated : IEvent
    {

        public Guid ArticleId { get; set; }
    }
}
