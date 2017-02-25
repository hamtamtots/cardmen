using NServiceBus;
using System;

namespace Cardmen.Messages.Commands
{
    public class BaseArticleOperationCommand : ICommand
    {

        public Guid ArticleId { get; set; }
    }
}
