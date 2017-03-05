using Cardmen.Messages;
using System.Threading.Tasks;

namespace Cardmen.Storage
{
    interface IArticleStorageRepository
    {

        Task InsertArticle(Article article);
    }
}
