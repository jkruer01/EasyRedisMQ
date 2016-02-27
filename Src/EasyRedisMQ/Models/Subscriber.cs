using EasyRedisMQ.Extensions;
using EasyRedisMQ.Services;
using StackExchange.Redis.Extensions.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EasyRedisMQ.Models
{
    public class Subscriber<T> where T : class
    {
        private ICacheClient _cacheClient;
        private IExchangeSubscriberService _exchangeSubscriberService;

        public Subscriber(ICacheClient cacheClient, IExchangeSubscriberService exchangeSubscriberService)
        {
            _cacheClient = cacheClient;
            _exchangeSubscriberService = exchangeSubscriberService;
        }

        public SubscriberInfo SubscriberInfo { get; set; }
        public Func<T, Task> OnMessageAsync { get; set; }

        public async Task InitializeAsync()
        {
            if (SubscriberInfo == null) throw new NullReferenceException("SubscriberInfo is required.");
            if (string.IsNullOrWhiteSpace(SubscriberInfo.SubscriberId)) throw new NullReferenceException("SubscriberId is required");
            if (string.IsNullOrWhiteSpace(SubscriberInfo.ExchangeName)) throw new NullReferenceException("ExchangeName is required");
            if (string.IsNullOrWhiteSpace(SubscriberInfo.QueueName)) throw new NullReferenceException("QueueName is required");
            if (OnMessageAsync == null) throw new NullReferenceException("OnMessageAsync is required");

            await _cacheClient.SubscribeAsync<string>(SubscriberInfo.ExchangeName, DoWorkAsync);

            DoWorkAsync("").FireAndForget();
        }

        private async Task<T> GetNextMessageAsync()
        {
            return await _cacheClient.ListGetFromRightAsync<T>(SubscriberInfo.QueueName);
        }

        private async Task DoWorkAsync(string arg)
        {
            var stopWatch = new Stopwatch();
            int numberOfMessagesProcessed = 0;
            while(true)
            {
                var message = await GetNextMessageAsync();
                if (message == null) break;

                numberOfMessagesProcessed++;
                try
                {
                    await OnMessageAsync(message);
                }
                catch(Exception)
                {
                    await PushAsync(message);
                    throw;
                }
            }
        }

        private async Task PushAsync(T message)
        {
            await _exchangeSubscriberService.PushMessageToSubscriberAsync(SubscriberInfo, message);
        }
    }
}
