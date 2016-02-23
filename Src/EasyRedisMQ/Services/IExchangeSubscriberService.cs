using EasyRedisMQ.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyRedisMQ.Services
{
    public interface IExchangeSubscriberService
    {
        Task<List<SubscriberInfo>> GetSubscriberInfosAsync<T>() where T : class;
        Task AddSubscriberAsync<T>(Subscriber<T> subScriber) where T : class;
        Task PushMessageToSubscriberAsync<T>(SubscriberInfo subscriberInfo, T message) where T : class;
    }
}
