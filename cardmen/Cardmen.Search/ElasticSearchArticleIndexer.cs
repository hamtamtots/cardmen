using Cardmen.Messages;
using Nest;
using System.Threading.Tasks;

namespace Cardmen.Search
{
    class ElasticSearchArticleIndexer : IArticleIndexer
    {

        private IElasticClient _elasticClient;


        public ElasticSearchArticleIndexer(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }


        public async Task IndexArticle(Article article)
        {
            await _elasticClient.IndexAsync(article);
        }
    }
}
