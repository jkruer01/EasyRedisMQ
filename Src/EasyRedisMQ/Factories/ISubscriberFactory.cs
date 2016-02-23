using EasyRedisMQ.Models;
using System;
using System.Threading.Tasks;

namespace EasyRedisMQ.Factories
{
    public interface ISubscriberFactory
    {
        Task<Subscriber<T>> CreateSubscriberAsync<T>(string subscriberId, Func<T, Task> onMessage) where T : class;
    }
}
