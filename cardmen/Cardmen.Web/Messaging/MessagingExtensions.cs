using Cardmen.Messages.Commands;
using Cardmen.Messages.Events;

namespace Cardmen.Web.Messaging
{
    static class MessagingExtensions
    {

        public static object GetSagaMappingKey(this BaseArticleOperationEvent articleEvent)
        {
            return $"{articleEvent.ArticleId}{articleEvent.OperationKey}";
        }


        public static object GetSagaMappingKey(this BaseArticleOperationCommand articleCommand)
        {
            return $"{articleCommand.ArticleId}{articleCommand.OperationKey}";
        }


        public static object GetSagaMappingKey(this ArticleOperationData operationData)
        {
            return $"{operationData.ArticleId}{operationData.OperationKey}";
        }
    }
}
