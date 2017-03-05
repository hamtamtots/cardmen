using Cardmen.Messages;
using System.Threading.Tasks;

namespace Cardmen.Search
{
    interface IArticleIndexer
    {

        Task IndexArticle(Article article);
    }
}
