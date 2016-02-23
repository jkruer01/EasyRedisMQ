using EasyRedisMQ.Clients;
using EasyRedisMQ.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ICacheClientExtended _cacheClient;
        private readonly IExchangeNameResolver _exchangeNameResolver;

        public NotificationService(ICacheClientExtended cacheClient, IExchangeNameResolver exchangeNameResolver)
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
