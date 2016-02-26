using EasyRedisMQ.Resolvers;
using System;
using System.Threading.Tasks;
using EasyRedisMQ.Models;
using EasyRedisMQ.Services;
using StackExchange.Redis.Extensions.Core;

namespace EasyRedisMQ.Factories
{
    public class SubscriberFactory : ISubscriberFactory
    {
        private IQueueNameResolver _queueNameResolver;
        private IExchangeNameResolver _exchangeNameResolver;
        private ICacheClient _cacheClient;
        private IExchangeSubscriberService _exchangeSubscriberService;

        public SubscriberFactory(IQueueNameResolver queueNameResolver, 
            IExchangeNameResolver exchangeNameResolver, 
            ICacheClient cacheClient, 
            IExchangeSubscriberService exchangeSubscriberService)
        {
            _queueNameResolver = queueNameResolver;
            _exchangeNameResolver = exchangeNameResolver;
            _cacheClient = cacheClient;
            _exchangeSubscriberService = exchangeSubscriberService;
        }

        public async Task<Subscriber<T>> CreateSubscriberAsync<T>(string subscriberId, Func<T, Task> onMessageAsync) where T : class
        {
            var exchangeName = _exchangeNameResolver.GetExchangeName<T>();
            var queueName = _queueNameResolver.GetQueueName(exchangeName, subscriberId);
            var subscriber = new Subscriber<T>(_cacheClient, _exchangeSubscriberService)
            {
                SubscriberInfo = new SubscriberInfo
                {
                    SubscriberId = subscriberId,
                    ExchangeName = exchangeName,
                    QueueName = queueName
                },
                OnMessageAsync = onMessageAsync
            };

            await subscriber.InitializeAsync();

            return subscriber;
        }
    }
}
