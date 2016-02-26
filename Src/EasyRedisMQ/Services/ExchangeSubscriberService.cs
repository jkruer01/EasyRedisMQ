using System.Collections.Generic;
using System.Threading.Tasks;
using EasyRedisMQ.Models;
using StackExchange.Redis.Extensions.Core;
using EasyRedisMQ.Resolvers;
using System.Linq;

namespace EasyRedisMQ.Services
{
    public class ExchangeSubscriberService : IExchangeSubscriberService
    {
        private readonly ICacheClient _cacheClient;
        private IExchangeSubscribersResolver _exchangeSubscribersResolver;

        public ExchangeSubscriberService(ICacheClient cacheClient, IExchangeSubscribersResolver exchangeSubscribersResolver)
        {
            _cacheClient = cacheClient;
            _exchangeSubscribersResolver = exchangeSubscribersResolver;
        }

        public async Task AddSubscriberAsync<T>(Subscriber<T> subScriber) where T : class
        {
            var subscriberKey = _exchangeSubscribersResolver.GetSubscriberKey<T>();
            var wasSuccessful = await _cacheClient.SetAddAsync<SubscriberInfo>(subscriberKey, subScriber.SubscriberInfo);
        }

        public async Task<List<SubscriberInfo>> GetSubscriberInfosAsync<T>() where T : class
        {
            var subscriberKey = _exchangeSubscribersResolver.GetSubscriberKey<T>();
            var subscribers = await _cacheClient.SetMembersAsync<SubscriberInfo>(subscriberKey);
            return subscribers.ToList();
        }

        public async Task PushMessageToSubscriberAsync<T>(SubscriberInfo subscriberInfo, T message) where T : class
        {
            await _cacheClient.ListAddToLeftAsync(subscriberInfo.QueueName, message);
        }
    }
}
