using Microsoft.AspNetCore.SignalR.Infrastructure;
using System;

namespace Cardmen.Web.Server.Hubs
{
    public class ArticleHubClientProxy : IArticleClientProxy
    {

        private IConnectionManager _connectionManager;


        public ArticleHubClientProxy(IConnectionManager connectionManager)
        {
            this._connectionManager = connectionManager;
        }


        public void NotifyArticleOperationSuccessful(Guid articeId, string operationKey)
        {
            _connectionManager.GetHubContext<ArticleHub, IArticleHubClient>()
                .Clients.Client(operationKey)
                .NotifyArticleOperationSuccessful(articeId);
        }
    }
}
