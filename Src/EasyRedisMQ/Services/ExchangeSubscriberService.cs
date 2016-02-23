using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRedisMQ.Models;
using StackExchange.Redis.Extensions.Core;
using EasyRedisMQ.Resolvers;
using EasyRedisMQ.Clients;

namespace EasyRedisMQ.Services
{
    public class ExchangeSubscriberService : IExchangeSubscriberService
    {
        private readonly ICacheClientExtended _cacheClient;
        private IExchangeSubscribersResolver _exchangeSubscribersResolver;

        public ExchangeSubscriberService(ICacheClientExtended cacheClient, IExchangeSubscribersResolver exchangeSubscribersResolver)
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
            return await _cacheClient.SetMembersAsync<SubscriberInfo>(subscriberKey);
        }

        public async Task PushMessageToSubscriberAsync<T>(SubscriberInfo subscriberInfo, T message) where T : class
        {
            await _cacheClient.ListAddToLeftAsync(subscriberInfo.QueueName, message);
        }
    }
}
