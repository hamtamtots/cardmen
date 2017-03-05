using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cardmen.Web.Server.Hubs
{
    public class ArticleHub : Hub<IArticleHubClient>
    {

        ILogger _logger;
        IArticleRepository _articleRepository;


        public ArticleHub(ILoggerFactory loggerFactory, IArticleRepository articleRepo)
        {
            _logger = loggerFactory.CreateLogger<ArticleHub>();
            _articleRepository = articleRepo;
        }


        public async Task CreateArticle()
        {
            var newArticleId = Guid.NewGuid();
            _logger.LogInformation($"New article create requested, id: {newArticleId}");
            await _articleRepository.CreateArticleAsync(newArticleId, Context.ConnectionId);
        }
    }
}
