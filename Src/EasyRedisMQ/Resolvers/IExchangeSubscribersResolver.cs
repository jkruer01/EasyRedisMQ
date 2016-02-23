using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Resolvers
{
    public interface IExchangeSubscribersResolver
    {
        string GetSubscriberKey(string exchangeName);
        string GetSubscriberKey(object message);
        string GetSubscriberKey<T>() where T : class;
    }
}
