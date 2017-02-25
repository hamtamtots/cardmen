using NServiceBus;
using System;

namespace Cardmen.Messages.Events
{
    public class ArticleIndexUpdated : IEvent
    {

        public Guid ArticleId { get; set; }
    }
}
