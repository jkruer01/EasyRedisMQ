using EasyRedisMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(T message) where T : class;
        Task<Subscriber<T>> SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessageAsync) where T : class;
    }
}
