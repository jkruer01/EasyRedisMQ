using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Resolvers
{
    public class QueueNameResolver : IQueueNameResolver
    {
        public string GetQueueName(string exchangeName, string subscriberId)
        {
            return string.Format("{0}.Queue.{1}", exchangeName, subscriberId);
        }
    }
}
