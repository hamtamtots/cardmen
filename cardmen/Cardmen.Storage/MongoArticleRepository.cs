using Cardmen.Messages;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Cardmen.Storage
{
    class MongoArticleRepository : IArticleStorageRepository
    {

        private IMongoCollection<Article> _articleCollection;


        public MongoArticleRepository(IMongoCollection<Article> articleCollection)
        {
            _articleCollection = articleCollection;
        }


        public async Task InsertArticle(Article article)
        {
            await _articleCollection.InsertOneAsync(article);
        }
    }
}
