using EasyRedisMQ.Resolvers;
using StackExchange.Redis.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ICacheClient _cacheClient;
        private readonly IExchangeNameResolver _exchangeNameResolver;

        public NotificationService(ICacheClient cacheClient, IExchangeNameResolver exchangeNameResolver)
        {
            _cacheClient = cacheClient;
            _exchangeNameResolver = exchangeNameResolver;
        }

        public async Task NotifyOfNewMessagesAsync(object message)
        {
            var exchangeName = _exchangeNameResolver.GetExchangeName(message);
            await _cacheClient.PublishAsync(exchangeName, exchangeName);
        }
    }
}
