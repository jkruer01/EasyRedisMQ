using StackExchange.Redis.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRedisMQ.Clients
{
    public interface ICacheClientExtended : ICacheClient
    {
        Task<List<T>> SetMembersAsync<T>(string key);
    }
}
