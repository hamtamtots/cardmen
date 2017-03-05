using System;
using System.Threading.Tasks;

namespace Cardmen.Web.Server
{
    public interface IArticleRepository
    {

        Task CreateArticleAsync(Guid articleId, string operationKey);
    }
}
