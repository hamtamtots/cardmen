using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cardmen.Web.Server
{
    public interface IArticleClientProxy
    {

        void NotifyArticleOperationSuccessful(Guid articeId, string operationKey);
    }
}
