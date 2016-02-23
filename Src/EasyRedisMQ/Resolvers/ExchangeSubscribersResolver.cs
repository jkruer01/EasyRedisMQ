using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Resolvers
{
    public class ExchangeSubscribersResolver : IExchangeSubscribersResolver
    {
        private IExchangeNameResolver _exchangeNameResolver;

        public ExchangeSubscribersResolver(IExchangeNameResolver exchangeNameResolver)
        {
            _exchangeNameResolver = exchangeNameResolver;
        }

        public string GetSubscriberKey(object message)
        {
            var exchangeName = _exchangeNameResolver.GetExchangeName(message);
            return GetSubscriberKey(exchangeName);
        }

        public string GetSubscriberKey(string exchangeName)
        {
            return string.Format("{0}.Subscribers", exchangeName);
        }

        public string GetSubscriberKey<T>() where T : class
        {
            var exchangeName = _exchangeNameResolver.GetExchangeName<T>();
            return GetSubscriberKey(exchangeName);
        }
    }
}
