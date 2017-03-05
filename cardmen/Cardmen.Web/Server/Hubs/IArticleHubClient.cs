using System;

namespace Cardmen.Web.Server.Hubs
{
    public interface IArticleHubClient
    {

        void NotifyArticleOperationSuccessful(Guid articeId);
    }
}
