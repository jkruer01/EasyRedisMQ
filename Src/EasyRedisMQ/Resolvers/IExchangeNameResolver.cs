using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Resolvers
{
    public interface IExchangeNameResolver
    {
        string GetExchangeName(object message);
        string GetExchangeName<T>() where T : class;
    }
}
