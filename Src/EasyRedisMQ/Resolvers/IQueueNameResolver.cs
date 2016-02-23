using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Resolvers
{
    public interface IQueueNameResolver
    {
        string GetQueueName(string exchangeName, string subscriberId);
    }
}
